using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PasteleriaWebApp.Data.Extensions;
using PasteleriaWebApp.Data.Infrastructure;
using PasteleriaWebApp.Data.Services;
using PasteleriaWebApp.Models;
using PasteleriaWebApp.ViewModels;

namespace PasteleriaWebApp.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoServices services;
        private readonly ICategoria _categoriaDB;
        private readonly IProducto _productoDB;

        public ProductosController(ICategoria categoria, IProducto producto, ProductoServices productoServices)
        {
            _categoriaDB = categoria;
            _productoDB = producto;
            services = productoServices;
        }
        public IActionResult Index(int page = 1, string? categoria = null, string? producto = null)
        {
            var listaProductos = services.ListarClientes().Select(x => x.ToViewModel()).ToList();
            foreach (var item in listaProductos)
            {
                item.Categoria = services.ObtenerCategoriaPorID(item.CategoriaID).Nombre;
            }
            if (categoria != null)
                listaProductos = listaProductos.Where(p => p.CategoriaID == Convert.ToInt32(categoria)).ToList();
            if (producto != null)
                listaProductos = listaProductos.Where(p => p.Nombre.ToLower().Contains(producto.ToLower())).ToList();
            var listadoCategorias = _categoriaDB.Listar();
            int registrosPorPagina = 8;
            int totalProductos = listaProductos.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalProductos / registrosPorPagina));

            int registrosOmitir = registrosPorPagina * (page - 1);

            ViewBag.categorias = new SelectList(listadoCategorias, "ID", "Nombre", categoria);
            ViewBag.paginas = cantidadPaginas;
            ViewBag.paginaActual = page;
            ViewBag.categoriaActual = categoria;
            ViewBag.busquedaActual = producto;

            return View(listaProductos.Skip(registrosOmitir).Take(registrosPorPagina));
        }
        public IActionResult Detail(int id)
        {
            var productoBuscado = _productoDB.ObtenerPorID(id);
            return View(productoBuscado);
        }
        public IActionResult Create()
        {
            var categorias = _categoriaDB.Listar();
            ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
            return View(new ProductoVM());
        }

        [HttpPost]
        public IActionResult Create(ProductoVM model)
        {
            if (!ModelState.IsValid)
            {
                var categorias = _categoriaDB.Listar();
                ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
                return View(model);
            }

            var producto = new Producto
            {
                Nombre = model.Nombre,
                Precio = model.Precio,
                StockActual = model.StockActual,
                CategoriaID = model.CategoriaID
            };

            var exito = _productoDB.Registrar(producto);
            return RedirectToAction("Index");
        }

            public IActionResult Edit(int id)
        {
            var productoBuscado = _productoDB.ObtenerPorID(id);
            var categorias = _categoriaDB.Listar();
            ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
            return View(productoBuscado);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            var exito = _productoDB.Modificar(producto);
            if (exito)
                return RedirectToAction("Detail", new { id = producto.ID });
            return View(producto);
        }
    }
}
