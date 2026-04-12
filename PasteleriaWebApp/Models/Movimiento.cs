namespace PasteleriaWebApp.Models
{
    public class Movimiento
    {
        public int ID { get; set; }
        public int ProductoID { get; set; }
        public int UsuarioID { get; set; }
        public string TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Producto Producto { get; set; }
        public Usuario Usuario { get; set; }
    }
}
