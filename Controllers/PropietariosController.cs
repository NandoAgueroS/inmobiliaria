using System.Configuration;
using System.Diagnostics;
using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
        try
            {
              ViewBag.ControllerName = "Propietarios";
            if (TempData.ContainsKey("Accion"))
                ViewBag.Accion = TempData["Accion"];
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
             IList<Propietario> propietarios = repositorioPropietario.ListarTodos();

             return View(propietarios);
            }
            catch (MySqlException ex)
            {
             ViewBag.Error = "Ocurrió un error al recuperar los datos!";
                Console.WriteLine(ex.ToString());

            return View(new List<Propietario>());
            }
            catch (Exception e)
            {
               ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return View(new List<Propietario>());
            }
        
    }
    public IActionResult Formulario(int Id)
    {
        try
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
    public IActionResult Guardar(Propietario propietario)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Los datos ingresados no son válidos";
            return View(nameof(Formulario), propietario);
         }
               
        try
        {

            if (propietario.Id == 0)
            {
                repositorioPropietario.Alta(propietario);
                TempData["Accion"] = Accion.Alta.value;
                TempData["Id"] = propietario.Id;

            }
            else
            {
                repositorioPropietario.Modificacion(propietario);
                TempData["Accion"] = Accion.Modificacion.value;
                TempData["Id"] = propietario.Id;
            }
            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            ViewBag.Error = "Ocurrió un error al guardar el propietario!";
            Console.WriteLine(ex.ToString());

            return View(nameof(Formulario), propietario);
        }
        catch (Exception e)
        {
            ViewBag.Error = "Ocurrió un error inesperado al guardar al propietario";
            Console.WriteLine(e.ToString());

            return View(nameof(Formulario), propietario);
        }
        


    }
    
    public IActionResult Eliminar(int id)
    {
        try
            {
            repositorioPropietario.Baja(id);
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
     ;
    }
    
    public IActionResult Reactivar(int id)
    {
        try
            {
              repositorioPropietario.Reactivar(id);
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
    public IActionResult Buscar(string nombre)
    {
        try
            {
             IList<Propietario> propietario = repositorioPropietario.BuscarPorNombre(nombre);
             return Json(propietario);
            }
            catch (Exception ex)
            {
             ViewBag.Error = "Ocurrió un error al recuperar los datos!";
                Console.WriteLine(ex.ToString());

            return StatusCode(500,"Error inesperado");
            }
            
        
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
