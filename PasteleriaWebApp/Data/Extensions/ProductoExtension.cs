using PasteleriaWebApp.Models;
using PasteleriaWebApp.ViewModels;

namespace PasteleriaWebApp.Data.Extensions
{
    public static class ProductoExtension
    {
        public static ProductoVM ToViewModel(this Producto producto)
        {
            if (producto == null) throw new ArgumentNullException("El cliente no ha sido creado");

            return new ProductoVM
            {
                ID = producto.ID,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                StockActual = producto.StockActual,
                FechaVencimiento = producto.FechaVencimiento,
                CategoriaID = producto.CategoriaID
            };
        }
    }
}
