using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;
using inmobiliaria.Repositories;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria.Controllers;

[Authorize]
public class TiposController : Controller
{
    private readonly IRepositorioTipo repositorioTipo;

    public TiposController(IRepositorioTipo repositorioTipo)
    {
        this.repositorioTipo = repositorioTipo;
    }

    public IActionResult Index()
    {
        ViewBag.ControllerName = "Tipos";
        if (TempData.ContainsKey("Accion"))
            ViewBag.Accion = TempData["Accion"];
        if (TempData.ContainsKey("Id"))
            ViewBag.Id = TempData["Id"];
        if (TempData.ContainsKey("Error"))
            ViewBag.Error = TempData["Error"];
        try
        {
            IList<Tipo> tipos = repositorioTipo.ListarTodos();
            return View(tipos);
        }
        catch (MySqlException ex)
        {
            ViewBag.Error = "Ocurrió un error al recuperar los tipos";

            Console.WriteLine(ex.ToString());

            return View(new List<Tipo>());
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Ocurrió un error inesperado";
            Console.WriteLine(ex.ToString());
            return View(new List<Tipo>());
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
                Tipo tipo = repositorioTipo.BuscarPorId(id);
                return View(tipo);
            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar el tipo";
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
    public IActionResult Guardar(Tipo tipo)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Los datos ingresados no son válidos";
            return View(nameof(Formulario), tipo);
        }
        try
        {

            if (tipo.Id == 0)
            {
                repositorioTipo.Alta(tipo);
                TempData["Accion"] = Accion.Alta.value;
            }
            else
            {
                repositorioTipo.Modificacion(tipo);
                TempData["Accion"] = Accion.Modificacion.value;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            ViewBag.Error = "Ocurrió un error al guardar el tipo";
            Console.WriteLine(ex.ToString());
            return View(nameof(Formulario), tipo);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Ocurrió un error inesperado";
            Console.WriteLine(ex.ToString());
            return View(nameof(Formulario), tipo);
        }
    }

    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        try
        {

            repositorioTipo.Baja(id);
            TempData["Accion"] = Accion.Baja.value;
            TempData["Id"] = id;


            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            TempData["Error"] = "Ocurrió un error al eliminar el tipo";
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
        repositorioTipo.Reactivar(id);
        return RedirectToAction(nameof(Index));
    }


    public IActionResult BuscarPorId(int id)
    {
        try
        {
            Tipo tipo = repositorioTipo.BuscarPorId(id);
            return Json(tipo);
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
