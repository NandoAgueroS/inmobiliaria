using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Controllers
{
    public class ContratosController : Controller
    {

        private readonly IRepositorioContrato repositorioContrato;
        private readonly IRepositorioPago repositorioPago;
        private readonly IRepositorioInmueble repositorioInmueble;

        public ContratosController(IRepositorioContrato repositorioContrato, IRepositorioPago repositorioPago, IRepositorioInmueble repositorioInmueble)
        {
            this.repositorioContrato = repositorioContrato;
            this.repositorioPago = repositorioPago;
            this.repositorioInmueble = repositorioInmueble;
        }

        public IActionResult Index()
        {
            ViewBag.ControllerName = "contratos";
            if (TempData.ContainsKey("Accion"))
                ViewBag.Accion = TempData["Accion"];
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            try
            {
                IList<Contrato> contratos = repositorioContrato.ListarTodos();
                return View(contratos);
            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar los contratos";

                Console.WriteLine(ex.ToString());

                return View(new List<Contrato>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(ex.ToString());
                return View(new List<Contrato>());
            }

        }

        public IActionResult Formulario(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                try
                {
                    Contrato contrato = repositorioContrato.BuscarPorId(id);
                    return View(contrato);
                }
                catch (MySqlException ex)
                {
                    ViewBag.Error = "Ocurrió un error al recuperar el contrato";
                    Console.WriteLine(ex.ToString());
                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error inesperado";
                    Console.WriteLine(ex.ToString());
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Guardar(Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Los datos ingresados no son válidos";
                return View(nameof(Formulario), contrato);
            }
            try
            {
                bool inmuebleDesocupado = repositorioInmueble.VerificarDesocupado(contrato.FechaDesde, contrato.FechaHasta, contrato.IdInmueble);
                bool inmuebleDisponible = repositorioInmueble.VerificarDisponible(contrato.IdInmueble);
                if (inmuebleDesocupado && inmuebleDisponible)
                {
                    if (contrato.Id == null)
                    {
                        repositorioContrato.Alta(contrato);
                        TempData["Accion"] = Accion.Alta.value;
                    }
                    else
                    {
                        repositorioContrato.Modificacion(contrato);
                        TempData["Accion"] = Accion.Modificacion.value;
                    }

                    return RedirectToAction(nameof(Index));
                }
                else if (!inmuebleDesocupado && inmuebleDisponible)
                {
                    ViewBag.Error = "El inmueble se encuentra ocupado en el periodo seleccionado";
                    return View(nameof(Formulario), contrato);
                }
                else
                {
                    ViewBag.Error = "El inmueble no se encuentra disponible";
                    return View(nameof(Formulario), contrato);
                }
            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al guardar el contrato";
                Console.WriteLine(ex.ToString());
                return View(nameof(Formulario), contrato);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(ex.ToString());
                return View(nameof(Formulario), contrato);
            }
        }


        public IActionResult CancelarModal(int id)
        {
            Contrato contrato = repositorioContrato.BuscarPorId(id);
            Pago ultimoPago = repositorioPago.BuscarUltimoPago(id);

            int mesesTotales = ((contrato.FechaHasta.Year - contrato.FechaDesde.Year) * 12 + contrato.FechaHasta.Month - contrato.FechaDesde.Month);
            int mesesPagados = repositorioPago.ContarPagosMensuales(id) > 0 ? repositorioPago.ContarPagosMensuales(id) : 0;

            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Today);
            int mesesTranscurridos = (fechaActual.Year - contrato.FechaDesde.Year) * 12 + fechaActual.Month - contrato.FechaDesde.Month;
            int mesesImpagos = mesesTranscurridos - mesesPagados;
            mesesImpagos = mesesPagados < 0 ? 0 : mesesPagados;
            int mesesRestantes = (contrato.FechaHasta.Year - fechaActual.Year) * 12 + contrato.FechaHasta.Month - fechaActual.Month;
            decimal multa;
            if ((mesesTotales / 2) < mesesRestantes)
            {
                multa = contrato.Monto * 2;
            }
            else
            {
                multa = contrato.Monto;
            }
            /*mete todos los datos en view bags*/
            ViewBag.IdContrato = id;
            // ViewBag.Multa = multa;
            TempData["Multa"] = multa.ToString();
            ViewBag.MesesImpagos = mesesImpagos;
            TempData["MesesImpagos"] = mesesImpagos;
            TempData["DesdeMulta"] = true;
            ViewBag.MesesRestantes = mesesRestantes;
            ViewBag.MesesTotales = mesesTotales;
            ViewBag.MesesTranscurridos = mesesTranscurridos;

            return View();
        }
        public IActionResult Eliminar(int id)
        {
            try
            {
                Pago ultimoPago = repositorioPago.BuscarUltimoPago(id);
                if (ultimoPago != null && ultimoPago.Contrato != null && ultimoPago.CorrespondeAMes != null)
                {
                    Contrato contrato = ultimoPago.Contrato;

                    DateOnly fechaUltimoPago = ultimoPago.CorrespondeAMes.Value;
                    int mesesRestantes = ((contrato.FechaHasta.Year - fechaUltimoPago.Year) * 12 + contrato.FechaHasta.Month - fechaUltimoPago.Month);
                    if (fechaUltimoPago.Month != DateOnly.FromDateTime(DateTime.Today).Month
                        && fechaUltimoPago.Year != DateOnly.FromDateTime(DateTime.Today).Year)
                    {
                        TempData["Error"] = "No se puede cancelar el contrato, tiene pagos pendientes";
                    }
                }
                repositorioContrato.Baja(id);
                TempData["Accion"] = Accion.Baja.value;
                TempData["Id"] = id;


                return RedirectToAction(nameof(Index));
            }
            catch (MySqlException ex)
            {
                TempData["Error"] = "Ocurrió un error al eliminar el contrato";
                Console.WriteLine(ex.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error inesperado";
                Console.WriteLine(ex.ToString());
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Reactivar(int id)
        {
            repositorioContrato.Reactivar(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult BuscarPorId(int id)
        {
            try
            {
                Contrato contrato = repositorioContrato.BuscarPorId(id);
                return Json(contrato);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());

                return StatusCode(500, "Error en la base de datos");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                return StatusCode(500, "Error general");
            }
        }
        public IActionResult RenovarModal(int id)
        {
            if (id == 0)
                return View(nameof(Index));
            Contrato contratoOriginal = repositorioContrato.BuscarPorId(id);
            RenovarContratoDTO contratoDTO = new RenovarContratoDTO
            {
                IdOriginal = contratoOriginal.Id.Value,
                FechaDesde = contratoOriginal.FechaDesde,
                FechaHasta = contratoOriginal.FechaHasta,
                NuevoMonto = contratoOriginal.Monto
            };
            ViewBag.IdInmueble = contratoOriginal.IdInmueble;
            return View(contratoDTO);
        }
        public IActionResult Renovar(RenovarContratoDTO contratoDTO)
        {
            Contrato contratoARenovar = repositorioContrato.BuscarPorId(contratoDTO.IdOriginal);
            contratoARenovar.Monto = contratoDTO.NuevoMonto;
            contratoARenovar.FechaDesde = contratoDTO.FechaDesde;
            contratoARenovar.FechaHasta = contratoDTO.FechaHasta;

            if (repositorioInmueble.VerificarDesocupado(contratoARenovar.FechaDesde, contratoARenovar.FechaHasta, contratoARenovar.IdInmueble))
                repositorioContrato.Alta(contratoARenovar);
            else
                TempData["Error"] = "El inmueble se encuentra ocupado en el periodo seleccionado";
            return RedirectToAction(nameof(Index));

        }
    }
}
