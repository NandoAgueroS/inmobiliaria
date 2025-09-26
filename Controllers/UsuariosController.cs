using System.Security.Claims;
using inmobiliaria.Models;
using inmobiliaria.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
namespace inmobiliaria.Controllers
{
    [Authorize]
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
            Usuario usuario = repositorioUsuario.BuscarPorId(8);
            return View(usuario);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Index()
        {
            ViewBag.ControllerName = "Usuarios";
            if (TempData.ContainsKey("Accion"))
                ViewBag.Accion = TempData["Accion"];
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            var usuarios = repositorioUsuario.ListarTodos();
            return View(usuarios);
        }

        [Authorize(Policy = "Adminitrador")]
        public IActionResult Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(nameof(Formulario), usuario);
            try
            {
                string contraHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                   password: usuario.Clave,
                                   salt: System.Text.Encoding.ASCII.GetBytes(configuration["salt"]),
                                   prf: KeyDerivationPrf.HMACSHA1,
                                   iterationCount: 1000,
                                   numBytesRequested: 256 / 8));

                usuario.Clave = contraHash;
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
                Console.WriteLine(ex);
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(nameof(Formulario), usuario);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditarAvatar(Usuario usuarioConAvatar)
        {
            if (usuarioConAvatar.AvatarFile != null && usuarioConAvatar.Id.HasValue)
            {
                try
                {
                    Usuario usuario = repositorioUsuario.BuscarPorId(usuarioConAvatar.Id.Value);
                    if (usuario == null)
                    {
                        TempData["Error"] = "Usuario no encontrado.";
                        return RedirectToAction(nameof(Perfil));
                    }

                    if (!string.IsNullOrEmpty(usuario.Avatar))
                    {
                        var rutaAntigua = Path.Combine(environment.WebRootPath, usuario.Avatar.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAntigua))
                        {
                            System.IO.File.Delete(rutaAntigua);
                        }
                    }

                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuarioConAvatar.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/uploads", fileName);

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuarioConAvatar.AvatarFile.CopyTo(stream);
                    }
                    repositorioUsuario.Modificacion(usuario);
                    if (User.Identity.Name == usuario.Email)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.Email),
                            new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                            new Claim(ClaimTypes.Role, usuario.RolNombre),
                            new Claim("Avatar", usuario.Avatar ?? ""),
                            new Claim("IdUsuario", usuario.Id.Value.ToString()),
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    }


                    TempData["Accion"] = Accion.Modificacion.value;
                    if (User.Identity.Name == usuario.Email)
                        return RedirectToAction(nameof(Perfil));
                    else
                        return RedirectToAction(nameof(Perfil), new { Id = usuarioConAvatar.Id.Value });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    TempData["Error"] = "Ocurrió un error al cambiar el avatar.";
                    return RedirectToAction(nameof(Perfil));
                }
            }
            else
            {
                TempData["Error"] = "No se ha seleccionado ningún archivo.";
                return RedirectToAction(nameof(Perfil));
            }
        }
        public ActionResult CambiarClave(int id)
        {
            return View(new CambiarClaveDTO { IdUsuario = id });
        }

        [HttpPost]
        public ActionResult CambiarClave(CambiarClaveDTO cambiarClaveDTO)
        {
            if (!ModelState.IsValid && cambiarClaveDTO.Nueva != cambiarClaveDTO.NuevaConfirmada)
            {
                ViewBag.Error = "Ingresó datos incorrectos";
                return View(nameof(CambiarClave), cambiarClaveDTO);
            }
            try
            {
                Usuario usuario = repositorioUsuario.BuscarPorId(cambiarClaveDTO.IdUsuario);

                string nuevaHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                   password: cambiarClaveDTO.Nueva,
                                   salt: System.Text.Encoding.ASCII.GetBytes(configuration["salt"]),
                                   prf: KeyDerivationPrf.HMACSHA1,
                                   iterationCount: 1000,
                                   numBytesRequested: 256 / 8));

                if (!User.IsInRole("Administrador"))
                {
                    string antiguaHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                      password: cambiarClaveDTO.Antigua,
                                      salt: System.Text.Encoding.ASCII.GetBytes(configuration["salt"]),
                                      prf: KeyDerivationPrf.HMACSHA1,
                                      iterationCount: 1000,
                                      numBytesRequested: 256 / 8));
                    if (antiguaHash != usuario.Clave)
                    {
                        ViewBag.Error = "Ingresó datos incorrectos";
                        return View(nameof(CambiarClave), cambiarClaveDTO);
                    }
                }

                usuario.Clave = nuevaHash;
                repositorioUsuario.Modificacion(usuario);


                TempData["Accion"] = Accion.Modificacion.value;
                return RedirectToAction(nameof(Perfil));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(cambiarClaveDTO);
            }

        }

        [HttpPost]
        public async Task<ActionResult> EditarPerfil(Usuario usuario)
        {
            try
            {

                Usuario usuarioOriginal = repositorioUsuario.BuscarPorId(usuario.Id.Value);
                string mailViejo = usuarioOriginal.Email;
                if (usuarioOriginal != null)
                {
                    usuarioOriginal.Nombre = usuario.Nombre;
                    usuarioOriginal.Apellido = usuario.Apellido;
                    usuarioOriginal.Email = usuario.Email;
                }
                repositorioUsuario.Modificacion(usuario);
                if (User.Identity.Name == mailViejo)
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.Email),
                            new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                            new Claim(ClaimTypes.Role, usuarioOriginal.RolNombre),
                            new Claim("Avatar", usuarioOriginal.Avatar ?? ""),
                            new Claim("IdUsuario", usuarioOriginal.Id.Value.ToString()),
                        };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }
                TempData["Accion"] = Accion.Modificacion.value;
                return RedirectToAction(nameof(Perfil));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ViewBag.Error = "Ocurrió un error al actualizar el perfil";
                return View(nameof(Perfil), usuario);
            }
        }

        public IActionResult Perfil(int id)
        {
            // var usuario = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            Usuario usuario;
            if (id == 0)
                usuario = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
            else
            {
                usuario = repositorioUsuario.BuscarPorId(id);
                if (usuario == null)
                {
                    TempData["Error"] = "No se encontró el usuario";
                    return RedirectToAction("Index", "Home");
                }
                if (id != int.Parse(User.FindFirst("IdUsuario").Value) && !User.IsInRole("Administrador"))
                {
                    TempData["Error"] = "No se puede editar a otros usuarios";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(usuario);
        }


        [Authorize(Policy = "Administrador")]
        public IActionResult Formulario(int id)
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            if (id == 0)
            {
                return View();
            }

            var u = repositorioUsuario.BuscarPorId(id);
            return View(u);
        }




        public async Task<ActionResult> EliminarAvatar(int id)
        {
            var ruta = Path.Combine(environment.WebRootPath, "uploads", $"avatar_{id}" + Path.GetExtension(User.FindFirst("Avatar").Value));
            if (System.IO.File.Exists(ruta))
                System.IO.File.Delete(ruta);
            Usuario usuario = repositorioUsuario.BuscarPorId(id);
            usuario.Avatar = "";
            repositorioUsuario.Modificacion(usuario);
            if (User.Identity.Name == usuario.Email)
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.Email),
                            new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                            new Claim(ClaimTypes.Role, usuario.RolNombre),
                            new Claim("Avatar", usuario.Avatar ?? ""),
                            new Claim("IdUsuario", usuario.Id.Value.ToString()),
                        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var ruta = Path.Combine(environment.WebRootPath, "uploads", $"avatar_{id}" + Path.GetExtension(User.FindFirst("Avatar").Value));
                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);

                repositorioUsuario.Baja(id);
                TempData["Accion"] = Accion.Baja.value;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Ocurrió un error al eliminar";
                return View(nameof(Index));
            }
        }

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
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
                        ViewBag.Error = "El email o la clave no son correctas";
                        return View();
                    }
                    var claims = new List<Claim>

                    {
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim("FullName", usuario.Nombre + usuario.Apellido),
                        new Claim(ClaimTypes.Role, usuario.RolNombre),
                        new Claim("Avatar", usuario.Avatar),
                        new Claim("IdUsuario", usuario.Id.Value.ToString()),

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
