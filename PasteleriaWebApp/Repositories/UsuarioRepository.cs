using System.Data;
using System.Data.SqlClient;
using PasteleriaWebApp.Models;
using Microsoft.Extensions.Configuration;

namespace PasteleriaWebApp.Repositories
{
    public class UsuarioRepository
    {
        private readonly string _cadena;

        // El constructor recibe la configuración para leer el appsettings.json
        public UsuarioRepository(IConfiguration config)
        {
            _cadena = config.GetConnectionString("DB");
        }

        public Usuario ValidarAcceso(string correo, string clave)
        {
            Usuario usr = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                // Buscamos al usuario y traemos el nombre de su Rol
                string query = "SELECT u.*, r.NombreRol FROM Usuarios u " +
                               "INNER JOIN Roles r ON u.IdRol = r.IdRol " +
                               "WHERE u.Correo = @c AND u.Password = @p";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@c", correo);
                cmd.Parameters.AddWithValue("@p", clave);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        usr = new Usuario
                        {
                            IdUsuario = (int)dr["IdUsuario"],
                            Nombre = dr["Nombre"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            NombreRol = dr["NombreRol"].ToString(),
                            IdRol = (int)dr["IdRol"]
                        };
                    }
                }
            }
            return usr;
        }
    }
}