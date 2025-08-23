using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers;

public class InquilinosController : Controller
{
    private RepositorioInquilino repositorioInquilino;
    public InquilinosController(IConfiguration config)
    {
        this.repositorioInquilino = new RepositorioInquilino(config);
    }
    
    public IActionResult Index()
    {
        IList<Inquilino> inquilinos = repositorioInquilino.ListarTodos();

        return View(inquilinos);
    }

    public IActionResult Formulario(int id)
    {

        if (id == 0)
        {
            return View();
        } else {
            Inquilino inquilino = repositorioInquilino.BuscarPorId(id);
            return View(inquilino);
        }
    }
    
    [HttpPost]
    public IActionResult Guardar(Inquilino inquilino)
    {
        if (inquilino.Id == 0)
        {
            repositorioInquilino.Alta(inquilino);
        }
        else
        {
            repositorioInquilino.Modificacion(inquilino);
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Eliminar(int id)
    {
        repositorioInquilino.Baja(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
