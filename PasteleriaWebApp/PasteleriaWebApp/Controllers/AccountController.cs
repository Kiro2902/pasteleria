using Microsoft.AspNetCore.Mvc;
using PasteleriaWebApp.Repositories;
using PasteleriaWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace PasteleriaWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioRepository _repo;

        public AccountController(IConfiguration config)
        {
            _repo = new UsuarioRepository(config);
        }

        // Esta es la página que el usuario verá para loguearse
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            var usuario = _repo.ValidarAcceso(correo, clave);

            if (usuario != null)
            {
                // Aquí creamos la "identidad" del usuario con sus Claims (Etiquetas)
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.NombreRol) // "Administrador" o "Empleado"
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas. Intenta de nuevo.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}