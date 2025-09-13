using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorioPago repositorioPago;
        private readonly IRepositorioContrato repositorioContrato;

        public PagosController(IRepositorioPago repositorioPago, IRepositorioContrato repositorioContrato)
        {
            this.repositorioPago = repositorioPago;
            this.repositorioContrato = repositorioContrato;
        }
         
         public IActionResult Index()
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
        
        public IActionResult Formulario(int id)
        {

            try
            {

                IList<Contrato> contrato = repositorioContrato.ListarTodos();
                ViewBag.Contrato = contrato;

                if (id == 0)
                {
                    return View();
                }
                else
                {
                    Pago pago = repositorioPago.BuscarPorId(id);
                    return View(pago);
                }
            }


            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar los datos!";
                Console.WriteLine(ex.ToString());

                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return View();
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
                if (pago.Id == 0)
                {
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

        public IActionResult Eliminar(int id)
        {
            try
            {
                repositorioPago.Baja(id);
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
    }
}