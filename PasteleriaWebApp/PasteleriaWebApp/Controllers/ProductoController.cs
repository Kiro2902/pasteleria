using Microsoft.AspNetCore.Mvc;
using PasteleriaWebApp.Models;
using PasteleriaWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace PasteleriaWebApp.Controllers
{
    [Authorize] // Bloquea el acceso si no están logueados
    public class ProductoController : Controller
    {
        private readonly ProductoRepository _productoDB;

        public ProductoController(ProductoRepository productoDB)
        {
            _productoDB = productoDB;
        }

        // Listado principal con Buscador y Paginación
        public IActionResult Index(int page = 1, string? buscar = null)
        {
            var listaProductos = _productoDB.Listar();

            if (!string.IsNullOrEmpty(buscar))
            {
                listaProductos = listaProductos.Where(p =>
                    p.Nombre.ToLower().Contains(buscar.ToLower())).ToList();
            }

            int registrosPorPagina = 8;
            int totalProductos = listaProductos.Count;
            int cantidadPaginas = (int)Math.Ceiling((double)totalProductos / registrosPorPagina);
            int registrosOmitir = registrosPorPagina * (page - 1);

            ViewBag.paginas = cantidadPaginas;
            ViewBag.paginaActual = page;
            ViewBag.busquedaActual = buscar;

            var resultado = listaProductos.Skip(registrosOmitir).Take(registrosPorPagina).ToList();

            return View(resultado);
        }

        // GET: Muestra la pantalla para retirar insumos
        public IActionResult Retirar(int id)
        {
            var producto = _productoDB.ObtenerPorID(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // POST: Procesa la resta de stock en SQL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Retirar(int IdProducto, int CantidadRetiro)
        {
            if (CantidadRetiro <= 0)
            {
                TempData["Error"] = "La cantidad debe ser mayor a cero.";
                return RedirectToAction("Retirar", new { id = IdProducto });
            }

            bool exito = _productoDB.RegistrarSalida(IdProducto, CantidadRetiro);

            if (exito)
            {
                TempData["Mensaje"] = "Stock actualizado correctamente para la producción.";
                return RedirectToAction("Index");
            }

            // Si falla por falta de stock
            ViewBag.Error = "No hay suficiente stock para realizar este retiro.";
            return View(_productoDB.ObtenerPorID(IdProducto));
        }
    }
}