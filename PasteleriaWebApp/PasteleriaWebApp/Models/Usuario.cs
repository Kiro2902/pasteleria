namespace PasteleriaWebApp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public int IdRol { get; set; }

        // Propiedad extra para guardar el nombre del rol (Admin o Empleado)
        public string NombreRol { get; set; }
    }
}
