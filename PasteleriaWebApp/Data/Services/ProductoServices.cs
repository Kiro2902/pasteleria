using PasteleriaWebApp.Data.Infrastructure;
using PasteleriaWebApp.Models;
using PasteleriaWebApp.ViewModels;

namespace PasteleriaWebApp.Data.Services
{
    public class ProductoServices
    {
        private readonly IProducto productoDB;
        private readonly ICategoria categoriaDB;

        public ProductoServices(IProducto producto, ICategoria categoria)
        {
            productoDB = producto;
            categoriaDB = categoria;
        }

        public List<Producto> ListarClientes()
        {
            return productoDB.Listar();
        }

        public Producto ObtenerProductoPorID(int id)
        {
            return productoDB.ObtenerPorID(id);
        }

        public List<Categoria> ListarCategoria()
        {
            return categoriaDB.Listar();
        }

        public Categoria ObtenerCategoriaPorID(int id)
        {
            return categoriaDB.ObtenerPorID(id);
        }
    }
}
