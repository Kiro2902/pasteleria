using Microsoft.Data.SqlClient;
using PasteleriaWebApp.Data.Infrastructure;
using PasteleriaWebApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PasteleriaWebApp.Data.Repositories
{
    public class ProductoRepository : IProducto
    {
        private readonly string cadenaConexion = string.Empty;

        public ProductoRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:DB"] ?? string.Empty;
        }

        public bool Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Producto> Listar()
        {
            List<Producto> listado = new List<Producto>();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT P.*, C.NombreCategoria AS NombreCategoria FROM Productos P INNER JOIN Categorias C ON P.IDCategoria = C.IDCategoria", conexion))
                {
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                listado.Add(convertirReaderEnProducto(lector));
                            }
                        }
                    }
                }
            }
            return listado;
        }

        public bool Modificar(Producto entity)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("UPDATE Productos SET Nombre = @nombre" +
                    "Precio = @precio, StockActual = @stockactual, StockMinimo = @stockminimo, FechaVencimiento = @fechavencimiento, IDCategoria = @categoria WHERE IDProducto = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", entity.Nombre);
                    comando.Parameters.AddWithValue("@preciocompra", entity.Precio);
                    comando.Parameters.AddWithValue("@stockactual", entity.StockActual);
                    comando.Parameters.AddWithValue("@stockminimo", entity.StockMinimo);
                    comando.Parameters.AddWithValue("@fechavencimiento", entity.FechaVencimiento);
                    comando.Parameters.AddWithValue("@diasalerta", entity.DiasAlerta);
                    comando.Parameters.AddWithValue("@categoria", entity.CategoriaID);
                    comando.Parameters.AddWithValue("@ID", entity.ID);
                    conexion.Open();
                    exito = comando.ExecuteNonQuery() > 0;
                }
            }
            return exito;
        }

        public Producto ObtenerPorID(int id)
        {
            var producto = new Producto();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("SELECT P.*, C.NombreCategoria AS NombreCategoria FROM Productos P INNER JOIN Categorias C ON P.IDCategoria = C.IDCategoria WHERE P.IDProducto = @ID", conexion))
                {
                    comando.Parameters.AddWithValue("@ID", id);
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            lector.Read();
                            producto = convertirReaderEnProducto(lector);
                        }
                    }
                }
            }
            return producto;
        }

        public bool Registrar(Producto entity)
        {
            var exito = false;
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                DateTime fechaParaGuardar = entity.FechaVencimiento;

                if (fechaParaGuardar < new DateTime(1753, 1, 1))
                {
                    fechaParaGuardar = DateTime.Now.AddMonths(6);
                }

                using (var comando = new SqlCommand("INSERT INTO Productos (Nombre, PrecioCompra, StockActual, StockMinimo, FechaVencimiento, DiasAlerta, IdCategoria) VALUES (@nombre, @precio, @stockActual, @stockMinimo, @fechaVencimiento, @diasAlerta, @IdCategoria)", conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", entity.Nombre);
                    comando.Parameters.AddWithValue("@precio", entity.Precio);
                    comando.Parameters.AddWithValue("@stockActual", entity.StockActual);
                    comando.Parameters.AddWithValue("@stockMinimo", entity.StockMinimo);
                    comando.Parameters.AddWithValue("@fechaVencimiento", fechaParaGuardar);
                    comando.Parameters.AddWithValue("@diasAlerta", entity.DiasAlerta);
                    comando.Parameters.AddWithValue("@IdCategoria", entity.CategoriaID);
                    conexion.Open();
                    exito = comando.ExecuteNonQuery() > 0;
                }
            }
            return exito;
        }

        #region . Métodos Privados .

        private Producto convertirReaderEnProducto(SqlDataReader lector)
        {
            return new Producto()
            {
                ID = lector.GetInt32(0),
                Nombre = lector.GetString(1),
                Precio = lector.GetDecimal(2),
                StockActual = lector.GetInt32(3),
                StockMinimo = lector.GetInt32(4),
                FechaVencimiento = lector.GetDateTime(5),
                DiasAlerta = lector.GetInt32(6),
                CategoriaID = lector.GetInt32(7),
                Categoria = new Categoria()
                {
                    ID = lector.GetInt32(7),
                    Nombre = lector.GetString(8)
                }
            };
        }

        #endregion
    }
}