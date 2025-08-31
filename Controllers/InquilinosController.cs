using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;
using inmobiliaria.Repositories;

namespace inmobiliaria.Controllers;

public class InquilinosController : Controller
{
    private readonly IRepositorioInquilino repositorioInquilino;
    public InquilinosController(IRepositorioInquilino repositorioInquilino)
    {
        this.repositorioInquilino = repositorioInquilino;
    }
    
    public IActionResult Index()
    {
        ViewBag.ControllerName = "Inquilinos";
        if (TempData.ContainsKey("Accion"))
            ViewBag.Accion = TempData["Accion"];
        if (TempData.ContainsKey("Id"))
            ViewBag.Id = TempData["Id"];
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
            TempData["Accion"] = Accion.Alta.value;
        }
        else
        {
            repositorioInquilino.Modificacion(inquilino);
            TempData["Accion"] = Accion.Modificacion.value;
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Eliminar(int id)
    {
        repositorioInquilino.Baja(id);
        TempData["Accion"] = Accion.Baja.value;
        TempData["Id"] = id;
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Reactivar(int id)
    {
        repositorioInquilino.Reactivar(id);
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Buscar(string nombre)
    {
        IList<Inquilino> inquilinos = repositorioInquilino.BuscarPorNombre(nombre);
        return Json(inquilinos);
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
