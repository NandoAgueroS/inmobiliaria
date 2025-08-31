using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Mvc;

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
            ViewBag.ControllerName = "Contratos";
            if (TempData.ContainsKey("Accion"))
                ViewBag.Accion = TempData["Accion"];
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            IList<Contrato> contratos = repositorioContrato.ListarTodos();

            return View(contratos);
        }

        public IActionResult Formulario(int id)
        {

            if (id == 0)
            {
                return View();
            }
            else
            {
                Contrato contrato = repositorioContrato.BuscarPorId(id);
                return View(contrato);
            }
        }

        [HttpPost]
        public IActionResult Guardar(Contrato contrato)
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

        public IActionResult Eliminar(int id)
        {
            repositorioContrato.Baja(id);
            TempData["Accion"] = Accion.Baja.value;
            TempData["Id"] = id;
            return RedirectToAction(nameof(Index));
        }
    
        public IActionResult Reactivar(int id)
        {
            repositorioContrato.Reactivar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}