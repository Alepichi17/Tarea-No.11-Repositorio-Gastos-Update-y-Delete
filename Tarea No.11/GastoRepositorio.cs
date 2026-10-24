using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_No._11
{
    public class GastoRepositorio
    {
        private const string CadenaConexion =
          @"Server=(localdb)\MSSQLLocalDB;" +
          @"Database=PruebaDB;" +
          @"Trusted_Connection=True;" +
          @"TrustServerCertificate=True;";

        public void Insertar(Gasto g)
        {
            string sql = "INSERT INTO Gastos (Descripcion, Monto, Categoria)" +
                "VALUES (@descripcion, @monto, @categoria)";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@descripcion", g.descripcion);
                comando.Parameters.AddWithValue("@monto", g.monto);
                comando.Parameters.AddWithValue("@categoria", g.categoria);

                conexion.Open();
                int Filas = comando.ExecuteNonQuery();
                Console.WriteLine(Filas == 1 ? "Gasto Ingresado" : "No Se Inserto");
            }
        }
        public List<Gasto> ListarGastos()
        {
            List<Gasto> lista = new List<Gasto>();
            string sql = "SELECT Id, Descripcion, Monto, Categoria " +
                "FROM Gastos ORDER BY Id";
            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Gasto
                        {
                            id = lector.GetInt32(0),
                            descripcion = lector.GetString(1),
                            monto = lector.GetDecimal(2),
                            categoria = lector.GetString(3)
                        });
                    }
                }
            }
            return lista;

        }
        public Gasto? BuscarPorId(int id)
        {
            string sql = "SELECT Id, Descripcion, Monto, Categoria " +
                "FROM Gastos WHERE Id = @id";
            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Gasto
                        {
                            id = lector.GetInt32(0),
                            descripcion = lector.GetString(1),
                            monto = lector.GetDecimal(2),
                            categoria = lector.GetString(3)
                        };
                    }
                }
            }
            return null;
        }

        public bool Actualizar(Gasto g)
        {
            string sql = "UPDATE Gastos " +
                "SET Descripcion = @descripcion, Monto = @monto, Categoria = @categoria " +
                "WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@descripcion", g.descripcion);
                comando.Parameters.AddWithValue("@monto", g.monto);
                comando.Parameters.AddWithValue("@categoria", g.categoria);
                comando.Parameters.AddWithValue("@id", g.id);
                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }
        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Gastos WHERE Id = @id";
            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }
    }
}
