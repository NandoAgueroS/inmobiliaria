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
        this.repositorioContrato= repositorioContrato;
    }
    
    public IActionResult Index()
    {
        IList<Contrato> contratos = repositorioContrato.ListarTodos();

        return View(contratos);
    }

    public IActionResult Formulario(int id)
    {

        if (id == 0)
        {
            return View();
        } else {
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
        }
        else
        {
            repositorioContrato.Modificacion(contrato);
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Eliminar(int id)
    {
        repositorioContrato.Baja(id);
        return RedirectToAction(nameof(Index));
    }
    }
}