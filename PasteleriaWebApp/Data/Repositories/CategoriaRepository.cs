using Microsoft.Data.SqlClient;
using PasteleriaWebApp.Data.Infrastructure;
using PasteleriaWebApp.Models;

namespace PasteleriaWebApp.Data.Repositories
{
    public class CategoriaRepository : ICategoria
    {
        private readonly string cadenaConexion = string.Empty;

        public CategoriaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:DB"] ?? string.Empty;
        }
        public bool Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Categoria> Listar()
        {
            var listaCategorias = new List<Categoria>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT * FROM Categorias", conexion))
                {
                    conexion.Open();
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                                listaCategorias.Add(ConvertirReaderEnObjeto(reader));
                        }
                    }
                }
            }
            return listaCategorias;
        }

        public bool Modificar(Categoria entity)
        {
            throw new NotImplementedException();
        }

        public Categoria ObtenerPorID(int id)
        {
            Categoria categoria = null;
            using var conexion = new SqlConnection(cadenaConexion);
            using var comando = new SqlCommand("ObtenerCategoria", conexion);
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@Id" +
                "" +
                "", id);
            conexion.Open();
            using var reader = comando.ExecuteReader();
            if (reader != null && reader.HasRows)
            {
                reader.Read();
                categoria = ConvertirReaderEnObjeto(reader);
            }
            return categoria;
        }

        public bool Registrar(Categoria entity)
        {
            throw new NotImplementedException();
        }

        #region . Métodos Privados .

        private Categoria ConvertirReaderEnObjeto(SqlDataReader lector)
        {
            return new Categoria
            {
                ID = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Descripcion = lector.GetString(2),
            };
        }

        #endregion
    }
}
