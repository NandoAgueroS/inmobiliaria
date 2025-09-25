using System;
using System.Configuration;
using System.Diagnostics;
using System.Security.Claims;
using inmobiliaria.Models;
using inmobiliaria.Repositories;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using AspNetCoreGeneratedDocument;
namespace inmobiliaria.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IWebHostEnvironment environment;


        public UsuariosController(IRepositorioUsuario repositorioUsuario, IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.repositorioUsuario = repositorioUsuario;
            this.configuration = configuration;
            this.environment = environment;
        }

        public IActionResult PerfilUsuario()
        {
           Usuario usuario= repositorioUsuario.BuscarPorId(8);
            return View(usuario);
        }
     
       /// [Authorize(Policy = "Administrador")]
       
        public IActionResult Index()
        {
            var usuarios = repositorioUsuario.ListarTodos();
            return View(usuarios);
        }
    

      
       /// [Authorize(Policy = "Administrador")]

        public IActionResult Detalles(int id)
        {
            var usuario = repositorioUsuario.BuscarPorId(id);
            return View(usuario);


        }

       

       /////////////// [Authorize(Policy = "Adminitrador")]
        public IActionResult Guardar(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(nameof(Formulario),usuario);
            try
            {
                string contraHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                   password: usuario.Clave,
                                   salt: System.Text.Encoding.ASCII.GetBytes(configuration["salt"]),
                                   prf: KeyDerivationPrf.HMACSHA1,
                                   iterationCount: 1000,
                                   numBytesRequested: 256 / 8));

                usuario.Clave = contraHash;
                usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado;
                var identificador = Guid.NewGuid();
                int res = repositorioUsuario.Alta(usuario);
                if (usuario.AvatarFile != null && usuario.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/uploads", fileName);

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(stream);
                    }
                    repositorioUsuario.Modificacion(usuario);


                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(nameof(Formulario),usuario);
            }

        }



        [Authorize]
        public IActionResult Perfil()
        {
            var usuario = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View( usuario);
        }


       ////// [Authorize(Policy = "Administrador")]
        public IActionResult Formulario(int id)
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            if(id==0)
            {
                return View();
            }
            
            var u = repositorioUsuario.BuscarPorId(id);
            return View(u);
        }




        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public IActionResult Eliminar(int id, Usuario usuario)
        {
            try
            {
                var ruta = Path.Combine(environment.WebRootPath, "uploads", $"avatar_{id}" + Path.GetExtension(usuario.Avatar));
                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);

                repositorioUsuario.Baja(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult Avatar()
        {
            var usuario = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
            string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.Avatar);
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "uploads");
            string pathCompleto = Path.Combine(path, fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(pathCompleto);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }

       
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioDTO userDTO)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string contraHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: userDTO.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                    var usuario = repositorioUsuario.ObtenerPorEmail(userDTO.Email);

                    if (usuario == null || usuario.Clave != contraHash)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctas");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }
                    var claims = new List<Claim>

                    {
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim("FullName", usuario.Nombre + usuario.Apellido),
                        new Claim(ClaimTypes.Role, usuario.RolNombre),

                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);

                }
                TempData["returnnUrl"] = returnUrl;
                return View();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        

        [Authorize(Policy = "Administrador")]
        public IActionResult Reactivar(int id)
        {
            try
            {
                repositorioUsuario.Reactivar(id);
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
