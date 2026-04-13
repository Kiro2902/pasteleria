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

        // Inyección de dependencias correcta
        public AccountController(UsuarioRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            var usuario = _repo.ValidarAcceso(correo, clave);

            if (usuario != null)
            {
                // Creamos los Claims con el NombreRol que viene de la base de datos
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.NombreRol)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Redirigimos al Inventario de Productos
                return RedirectToAction("Index", "Producto");
            }

            ViewBag.Error = "Credenciales incorrectas. Verifica tu correo y clave.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}