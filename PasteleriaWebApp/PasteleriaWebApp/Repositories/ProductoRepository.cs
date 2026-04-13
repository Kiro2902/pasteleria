using Microsoft.Data.SqlClient; 
using System.Data;             
using PasteleriaWebApp.Models;  
using Microsoft.Extensions.Configuration;

namespace PasteleriaWebApp.Repositories
{
    public class ProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DB")!;
        }

        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                // Unimos con Categorías para mostrar el nombre, no solo el ID
                string query = @"SELECT p.*, c.NombreCategoria 
                               FROM Productos p 
                               INNER JOIN Categorias c ON p.IdCategoria = c.IdCategoria";

                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = (int)dr["IdProducto"],
                            Nombre = dr["Nombre"].ToString()!,
                            PrecioCompra = (decimal)dr["PrecioCompra"],
                            StockActual = (int)dr["StockActual"],
                            StockMinimo = (int)dr["StockMinimo"],
                            FechaVencimiento = dr["FechaVencimiento"] != DBNull.Value ? (DateTime)dr["FechaVencimiento"] : null,
                            IdCategoria = (int)dr["IdCategoria"],
                            NombreCategoria = dr["NombreCategoria"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
        public bool RegistrarSalida(int idProducto, int cantidad)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                // Esta consulta resta el stock actual en la tabla Productos
                string query = "UPDATE Productos SET StockActual = StockActual - @cantidad WHERE IdProducto = @id AND StockActual >= @cantidad";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@id", idProducto);

                cn.Open();
                int filasAfectadas = cmd.ExecuteNonQuery();
                return filasAfectadas > 0; // Si es 0, es porque no había suficiente stock
            }
        }
        public Producto ObtenerPorID(int id)
        {
            Producto producto = null;
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Productos WHERE IdProducto = @id";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        producto = new Producto
                        {
                            IdProducto = (int)dr["IdProducto"],
                            Nombre = dr["Nombre"].ToString()!,
                            PrecioCompra = (decimal)dr["PrecioCompra"],
                            StockActual = (int)dr["StockActual"],
                            StockMinimo = (int)dr["StockMinimo"],
                            FechaVencimiento = dr["FechaVencimiento"] != DBNull.Value ? (DateTime)dr["FechaVencimiento"] : null,
                            IdCategoria = (int)dr["IdCategoria"]
                        };
                    }
                }
            }
            return producto!;
        }
    }
}