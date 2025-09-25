using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IRepositorioInmueble repositorioInmueble;
        private readonly IRepositorioTipo repositorioTipo;
        public InmueblesController(IRepositorioInmueble repositorioInmueble, IRepositorioTipo repositorioTipo)
        {
            this.repositorioInmueble = repositorioInmueble;
            this.repositorioTipo = repositorioTipo;
        }
        public IActionResult Index(bool soloLibres, bool soloDisponibles)
        {
            try
            {
                ViewBag.ControllerName = "Inmuebles";
                if (TempData.ContainsKey("Accion"))
                    ViewBag.Accion = TempData["Accion"];
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                // IList<Inmueble> inmuebles = repositorioInmueble.ListarTodos();
                return View();

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



        public IActionResult Formulario(int id)
        {

            try
            {

                IList<Tipo> tipos = repositorioTipo.ListarTodos();
                ViewBag.Tipos = tipos;

                if (id == 0)
                {
                    return View();
                }
                else
                {
                    Inmueble inmueble = repositorioInmueble.BuscarPorId(id);
                    return View(inmueble);
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
        public IActionResult Guardar(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Los datos ingresados no son válidos";
                return View(nameof(Formulario), inmueble);
            }

            try
            {
                if (inmueble.Id == null)
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
                ViewBag.Error = "Ocurrió un error al guardar!";
                Console.WriteLine(ex.ToString());

                return View(nameof(Formulario), inmueble);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado";
                Console.WriteLine(e.ToString());

                return View(nameof(Formulario), inmueble);
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
        public IActionResult Buscar(string direccion)
        {
            try
            {
                IList<Inmueble> inmuebles = repositorioInmueble.BuscarPorDireccion(direccion);
                return Ok(inmuebles);
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
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                Inmueble inmueble = repositorioInmueble.BuscarPorId(id);
                return Ok(inmueble);
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

        public IActionResult Listar(bool? disponibles, DateOnly? fechaDesde, DateOnly? fechaHasta)
        {
            try
            {
                IList<Inmueble> inmuebles = new List<Inmueble>();
                if (disponibles == null)
                {
                    inmuebles = repositorioInmueble.ListarTodos();
                }
                else if (fechaDesde != null && fechaHasta != null)
                {
                    inmuebles = repositorioInmueble.ListarDesocupados(fechaDesde.Value, fechaHasta.Value, disponibles.Value);
                }
                else
                {
                    inmuebles = repositorioInmueble.ListarPorDisponible(disponibles.Value);
                }
                return Ok(inmuebles);
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
        [HttpGet("/Inmuebles/Desocupado/{id}/{fechaDesde}/{fechaHasta}")]
        public IActionResult Desocupado(int id, DateOnly fechaDesde, DateOnly fechaHasta)
        {
            return Json(repositorioInmueble.VerificarDesocupado(fechaDesde, fechaHasta, id));
        }

        [HttpPost]
        public IActionResult AltaTipo([FromBody] Tipo tipo)
        {
            try
            {
                repositorioTipo.Alta(tipo);
                return Created();
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
}
