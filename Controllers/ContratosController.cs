using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Controllers
{
    public class ContratosController : Controller
    {

        private readonly IRepositorioContrato repositorioContrato;
        public ContratosController(IRepositorioContrato repositorioContrato)
        {
            this.repositorioContrato = repositorioContrato;
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

                if (contrato.Id == 0)
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

        

        public IActionResult Eliminar(int id)
        {
            try
            {
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
    }
}