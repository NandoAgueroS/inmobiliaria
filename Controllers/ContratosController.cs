using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Controllers
{
    [Authorize]
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

            return View();
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
                bool inmuebleDesocupado;
                if (contrato.Id.HasValue)
                {
                    // Al editar, excluimos el contrato actual de la verificación
                    inmuebleDesocupado = repositorioInmueble.VerificarDesocupado(contrato.FechaDesde, contrato.FechaHasta, contrato.IdInmueble, contrato.Id);
                }
                else
                {
                    inmuebleDesocupado = repositorioInmueble.VerificarDesocupado(contrato.FechaDesde, contrato.FechaHasta, contrato.IdInmueble);
                }

                bool inmuebleDisponible = repositorioInmueble.VerificarDisponible(contrato.IdInmueble);
                if (inmuebleDesocupado && inmuebleDisponible)
                {
                    if (contrato.Id == null)
                    {
                        contrato.CreadoPor = int.Parse(User.FindFirst("IdUsuario").Value);
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


        public IActionResult _CancelarModal(int id)
        {
            try
            {
                Contrato contrato = repositorioContrato.BuscarPorId(id);
                int mesesPagados = repositorioPago.ContarPagosMensuales(id);
                DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Today);
                int mesesTranscurridos = (fechaActual.Year - contrato.FechaDesde.Year) * 12 + fechaActual.Month - contrato.FechaDesde.Month;
                int mesesImpagos = mesesTranscurridos - mesesPagados;
                mesesImpagos = mesesImpagos < 0 ? 0 : mesesImpagos;

                decimal multa = 0;
                bool multaPagada = repositorioPago.BuscarMulta(id);
                if (!multaPagada)
                {
                    int mesesTotales = ((contrato.FechaHasta.Year - contrato.FechaDesde.Year) * 12 + contrato.FechaHasta.Month - contrato.FechaDesde.Month);
                    int mesesRestantes = (contrato.FechaHasta.Year - fechaActual.Year) * 12 + contrato.FechaHasta.Month - fechaActual.Month;
                    if ((mesesTotales / 2) < mesesRestantes)
                    {
                        multa = contrato.Monto * 2;
                    }
                    else
                    {
                        multa = contrato.Monto;
                    }
                    TempData["Multa"] = multa.ToString();
                }
                ViewBag.PuedeCancelar = (mesesImpagos == 0 && multaPagada);
                ViewBag.MesesImpagos = mesesImpagos;
                ViewBag.Multa = multa;
                ViewBag.IdContrato = id;
                var dto = new CancelarContratoDTO
                {
                    IdContrato = id,
                    FechaCancelacion = DateOnly.FromDateTime(DateTime.Today)
                };
                return PartialView(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Content($"<div class='alert alert-danger'>Error al calcular los datos de cancelación: {ex.Message}</div>");
            }
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        public IActionResult Eliminar(CancelarContratoDTO dto)
        {
            int id = dto.IdContrato;
            try
            {
                repositorioContrato.Baja(id, int.Parse(User.FindFirst("IdUsuario").Value), dto.FechaCancelacion);
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

        [Authorize(Policy = "Administrador")]
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

        public IActionResult _RenovarModal(int id)
        {
            if (id == 0)
                return View(nameof(Index));
            Contrato contratoOriginal = repositorioContrato.BuscarPorId(id);
            int diferencia = contratoOriginal.FechaHasta.DayNumber - contratoOriginal.FechaDesde.DayNumber;
            DateOnly nuevaFechaHasta = contratoOriginal.FechaHasta.AddDays(diferencia + 1);
            RenovarContratoDTO contratoDTO = new RenovarContratoDTO
            {
                IdOriginal = contratoOriginal.Id.Value,
                FechaDesde = contratoOriginal.FechaDesde.AddDays(1),
                FechaHasta = nuevaFechaHasta,
                NuevoMonto = contratoOriginal.Monto
            };
            ViewBag.IdInmueble = contratoOriginal.IdInmueble;
            return PartialView(contratoDTO);
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

        public IActionResult DataTable(int? idInmueble)
        {


            try
            {
                if (idInmueble == null)
                {

                    IList<Contrato> contratos = repositorioContrato.ListarTodos();
                    return PartialView("_DataTable", contratos);
                }
                else
                {
                    IList<Contrato> contratos = repositorioContrato.ListarPorInmueble(idInmueble.Value);
                    return PartialView("_DataTable", contratos);
                }
            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar los contratos";

                Console.WriteLine(ex.ToString());

                return PartialView("_DataTable", new List<Contrato>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(ex.ToString());
                return PartialView("_DataTable", new List<Contrato>());
            }
        }
    }
}
