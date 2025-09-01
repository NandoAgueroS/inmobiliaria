using System.Configuration;
using System.Diagnostics;
using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers;

public class PropietariosController : Controller
{
    private readonly IRepositorioPropietario repositorioPropietario;

    public PropietariosController(IRepositorioPropietario repositorioPropietario)
    {
        this.repositorioPropietario = repositorioPropietario;

    }

    public IActionResult Index()
    {
        IList<Propietario> propietarios = repositorioPropietario.ListarTodos();

        return View(propietarios);
    }
    public IActionResult Formulario(int Id)
    {
        if (Id == 0)
        {
            return View();
        }
        else
        {
            Propietario propietario = repositorioPropietario.BuscarPorId(Id);
            return View(propietario);
        }
    }
    [HttpPost]
    public IActionResult Guardar(Propietario propietario)
    {
        if (propietario.Id == 0)
        {
            repositorioPropietario.Alta(propietario);
            
        }
        else
        {
            repositorioPropietario.Modificacion(propietario);
        }
        return RedirectToAction(nameof(Index));


    }
    
    public IActionResult Eliminar(int Id)
    {
     repositorioPropietario.Baja(Id);
        return RedirectToAction(nameof(Index));
    }
    
    public IActionResult Reactivar(int id)
    {
        repositorioPropietario.Reactivar(id);
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Buscar(string nombre)
    {
        IList<Propietario> propietario = repositorioPropietario.BuscarPorNombre(nombre);
        return Json(propietario);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
