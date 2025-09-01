using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IRepositorioInmueble repositorioInmueble;
        public InmueblesController(IRepositorioInmueble repositorioInmueble)
        {
            this.repositorioInmueble = repositorioInmueble;
        }
        public IActionResult Index()
        {
            try
            {
                ViewBag.ControllerName = "Inmuebles";
                if (TempData.ContainsKey("Accion"))
                    ViewBag.Accion = TempData["Accion"];
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                IList<Inmueble> inmuebles = repositorioInmueble.ListarTodos();
                return View(inmuebles);

            }
            catch (MySqlException ex)
            {
                ViewBag.Error = "Ocurrió un error al recuperar los datos!";
                Console.WriteLine(ex.ToString());

                return View(new List<Inmueble>());
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return View(new List<Inmueble>());
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

                    Inmueble inmueble = repositorioInmueble.BuscarPorId(id);
                    return View(inmueble);
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




        }

        [HttpPost]
        public IActionResult Guardar(Inmueble inmueble)
        {
            try
            {
             if (inmueble.Id == 0)
            {
                repositorioInmueble.Alta(inmueble);
                TempData["Accion"] = Accion.Alta.value;
            }
            else
            {
                repositorioInmueble.Modificacion(inmueble);
                TempData["Accion"] = Accion.Modificacion.value;
            }

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

        public IActionResult Eliminar(int id)
        {
            try
            {
                repositorioInmueble.Baja(id);
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

           
        }
    
        public IActionResult Reactivar(int id)
        {
            try
            {
                repositorioInmueble.Reactivar(id);
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
    }
}