using System.ComponentModel.DataAnnotations;

namespace PasteleriaWebApp.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioCompra { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int IdCategoria { get; set; }

        // Propiedad de navegación (opcional pero recomendada)
        public string? NombreCategoria { get; set; }
    }
}
