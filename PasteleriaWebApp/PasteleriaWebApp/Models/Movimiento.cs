namespace PasteleriaWebApp.Models
{
    public class Movimiento
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string Tipo { get; set; } = "SALIDA"; // Por defecto salida para el empleado
        public string Motivo { get; set; } = "PRODUCCION";
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
