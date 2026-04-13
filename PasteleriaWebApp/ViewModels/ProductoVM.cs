using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PasteleriaWebApp.ViewModels
{
    public class ProductoVM
    {
        public int ID { get; set; }

        [DisplayName("Nombre del producto")]
        [MinLength(10, ErrorMessage = "La longitud mínima del nombre es de 10 caracteres.")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "El precio del producto debe ser de al menos s/ 1.00.")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal Precio { get; set; }

        [DisplayName("Stock Actual")]
        [Required(ErrorMessage = "Debe indicar el stock del producto.")]
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public DateTime FechaVencimiento { get; set; }

        [DisplayName("Categoría")]
        [Required(ErrorMessage = "Debe indicar la categoría del producto.")]
        public int CategoriaID { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
