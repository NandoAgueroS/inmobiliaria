using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly IRepositorioPago repositorioPago;
        private readonly IRepositorioContrato repositorioContrato;
        private readonly IRepositorioInquilino repositorioInquilino;


        public PagosController(IRepositorioPago repositorioPago, IRepositorioContrato repositorioContrato, IRepositorioInquilino repositorioInquilino)
        {
            this.repositorioPago = repositorioPago;
            this.repositorioContrato = repositorioContrato;
            this.repositorioInquilino = repositorioInquilino;
        }

        public IActionResult Index(int? idInquilino)
        {
            try
            {
                ViewBag.ControllerName = "Pagos";
                if (TempData.ContainsKey("Accion"))
                    ViewBag.Accion = TempData["Accion"];
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                if (idInquilino != null)
                {
                    Inquilino inquilino = repositorioInquilino.BuscarPorId(idInquilino.Value);
                    ViewBag.Inquilino = inquilino;
                }
                IList<Pago> pago = repositorioPago.ListarTodos();
                return View(pago);

            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar los datos!";
                Console.WriteLine(ex.ToString());

                return View(new List<Pago>());
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return View(new List<Pago>());
            }
        }

        public IActionResult Formulario(int idContrato)
        {

            try
            {

                Contrato contrato = repositorioContrato.BuscarPorId(idContrato);
                ViewBag.Contrato = contrato;

                if (idContrato == 0)
                {
                    ViewBag.Error = "Error al cargar la vista de pago";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    int numeroPago = repositorioPago.ObtenerUltimoNumeroPago(idContrato);
                    numeroPago = (numeroPago == -1 ? 0 : numeroPago) + 1;

                    Pago pago = new Pago
                    {
                        NumeroPago = numeroPago.ToString(),
                        IdContrato = idContrato
                    };

                    if (TempData["Multa"] is string multaStr && Decimal.TryParse(multaStr, out decimal multa))
                    {
                        pago.Monto = multa;
                        pago.Concepto = "Multa";
                        ViewBag.EsMulta = true;
                    }

                    return View(pago);
                }
            }


            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar los datos!";
                Console.WriteLine(ex.ToString());

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return RedirectToAction(nameof(Index));
            }
        }




        [HttpPost]
        public IActionResult Guardar(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Los datos ingresados no son válidos";
                return View(nameof(Formulario), pago);
            }

            try
            {
                if (pago.Id == null)
                {
                    pago.CreadoPor = int.Parse(User.FindFirst("IdUsuario").Value);
                    repositorioPago.Alta(pago);
                    TempData["Accion"] = Accion.Alta.value;
                }
                else
                {
                    repositorioPago.Modificacion(pago);
                    TempData["Accion"] = Accion.Modificacion.value;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al guardar!";
                Console.WriteLine(ex.ToString());

                return View(nameof(Formulario), pago);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return View(nameof(Formulario), pago);
            }


        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                repositorioPago.Baja(id, int.Parse(User.FindFirst("IdUsuario").Value));
                TempData["Accion"] = Accion.Baja.value;
                TempData["Id"] = id;
                return RedirectToAction(nameof(Index));

            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar los datos!";
                Console.WriteLine(ex.ToString());

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return RedirectToAction(nameof(Index));
            }


        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Reactivar(int id)
        {
            try
            {
                repositorioPago.Reactivar(id);
                return RedirectToAction(nameof(Index));

            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar los datos!";
                Console.WriteLine(ex.ToString());

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return RedirectToAction(nameof(Index));
            }



        }

        [HttpGet("Pagos/Ultimo/{idContrato}")]
        public IActionResult Ultimo(int idContrato)
        {

            Pago ultimoPago = repositorioPago.BuscarUltimoPago(idContrato);
            return Ok(ultimoPago);
        }

        [HttpGet("Pagos/Siguiente/{idContrato}")]
        public IActionResult Siguiente(int idContrato)
        {

            Pago ultimoPago = repositorioPago.BuscarUltimoPago(idContrato);
            if (ultimoPago == null)
            {
                Contrato contrato = repositorioContrato.BuscarPorId(idContrato);
                return Ok(contrato.FechaDesde);
            }
            else
            {
                return Ok(ultimoPago.CorrespondeAMes.Value.AddMonths(1));
            }
        }
        public IActionResult Listar(int? idInquilino)
        {
            try
            {
                IList<Pago> pagos = new List<Pago>();
                if (idInquilino == null)
                {
                    pagos = repositorioPago.ListarTodos();
                }
                else
                {
                    pagos = repositorioPago.ListarPorInquilino(idInquilino.Value);
                }
                return Ok(pagos);
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
    }
}
