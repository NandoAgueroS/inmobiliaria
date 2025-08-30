using System.Configuration;
using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers;

public class PropietariosController : Controller
{
    private RepositorioPropietario repositorioPropietario;

    public PropietariosController(IConfiguration config)
    {
        // this.repositorioPropietario = new RepositorioPropietario(config);

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
            repositorioPropietario.Modificar(propietario);
        }
        return RedirectToAction(nameof(Index));


    }
    
    public IActionResult Eliminar(int Id)
    {
     repositorioPropietario.Baja(Id);
        return RedirectToAction(nameof(Index));
    }
    
    
}
