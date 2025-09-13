using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;
using inmobiliaria.Repositories;
using MySql.Data.MySqlClient;
using System.Collections;

namespace inmobiliaria.Controllers;

public class InquilinosController : Controller
{
    private readonly IRepositorioInquilino repositorioInquilino;
    private readonly IRepositorioContrato repositorioContrato;

    public InquilinosController(IRepositorioInquilino repositorioInquilino, IRepositorioContrato repositorioContrato)
    {
        this.repositorioInquilino = repositorioInquilino;
        this.repositorioContrato = repositorioContrato;
    }

    public IActionResult Index()
    {
        ViewBag.ControllerName = "Inquilinos";
        if (TempData.ContainsKey("Accion"))
            ViewBag.Accion = TempData["Accion"];
        if (TempData.ContainsKey("Id"))
            ViewBag.Id = TempData["Id"];
        if (TempData.ContainsKey("Error"))
            ViewBag.Error = TempData["Error"];
        try
        {
            IList<Inquilino> inquilinos = repositorioInquilino.ListarTodos();
            return View(inquilinos);
        }
        catch (MySqlException ex)
        {
            ViewBag.Error = "Ocurrió un error al recuperar los inquilinos";

            Console.WriteLine(ex.ToString());

            return View(new List<Inquilino>());
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Ocurrió un error inesperado";
            Console.WriteLine(ex.ToString());
            return View(new List<Inquilino>());
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
                Inquilino inquilino = repositorioInquilino.BuscarPorId(id);
                return View(inquilino);
            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar el inquilino";
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
    public IActionResult Guardar(Inquilino inquilino)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Los datos ingresados no son válidos";
            return View(nameof(Formulario), inquilino);
        }
        try
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
        catch (MySqlException ex)
        {
            ViewBag.Error = "Ocurrió un error al guardar el inquilino";
            Console.WriteLine(ex.ToString());
            return View(nameof(Formulario), inquilino);
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Ocurrió un error inesperado";
            Console.WriteLine(ex.ToString());
            return View(nameof(Formulario), inquilino);
        }
    }

    public IActionResult Eliminar(int id)
    {
        try
        {
            Contrato contrato = repositorioContrato.BuscarActualPorInquilino(id);
            if (contrato != null)
            {
                TempData["Error"] = "Fallo al eliminar: el inquilino tiene un contrato activo";
                return RedirectToAction(nameof(Index));
            }

            repositorioInquilino.Baja(id);
            TempData["Accion"] = Accion.Baja.value;
            TempData["Id"] = id;


            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            TempData["Error"] = "Ocurrió un error al eliminar el inquilino";
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
        repositorioInquilino.Reactivar(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Buscar(string nombre)
    {
        IList<Inquilino> inquilinos = repositorioInquilino.BuscarPorNombre(nombre);
        return Json(inquilinos);
    }
    
    public IActionResult BuscarPorId(int id)
        {
            try
            {
                Inquilino inquilino = repositorioInquilino.BuscarPorId(id);
                return Json(inquilino);
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
