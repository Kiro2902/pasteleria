using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PasteleriaWebApp.Models
{
    public class Producto
    {
        public int ID { get; set; }

        [DisplayName("Nombre del producto")]
        [MinLength(10, ErrorMessage = "La longitud mínima del nombre es de 10 caracteres.")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string Nombre { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "El precio del producto debe ser de al menos s/ 1.00.")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal Precio { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public DateTime FechaVencimiento {  get; set; }
        public int DiasAlerta { get; set; }

        [DisplayName("Categoría")]
        [Required(ErrorMessage = "Debe indicar la categoría del producto.")]
        public int CategoriaID { get; set; }
        public Categoria Categoria { get; set; }
    }
}
