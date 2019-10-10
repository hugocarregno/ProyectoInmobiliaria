using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorio<Inmueble>
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }
        public int Alta(Inmueble i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inmuebles (Direccion, Uso, Tipo, CantidadHabitantes, Precio, Estado, IdPropietario) " +
                    $"VALUES ('{i.Direccion}','{i.Uso}','{i.Tipo}',{i.CantidadHabitantes},{i.Precio},'{i.Estado}',{i.IdPropietario})";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    i.IdInmueble = (int)command.ExecuteScalar();
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Inmuebles WHERE IdInmueble = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inmueble i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmuebles SET Direccion ='{i.Direccion}', Uso='{i.Uso}', Tipo='{i.Tipo}', CantidadHabitantes={i.CantidadHabitantes}, Precio={i.Precio}, Estado='{i.Estado}', IdPropietario='{i.IdPropietario}' " +
                    $"WHERE IdInmueble = {i.IdInmueble}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble i = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInmueble, Direccion, Uso, Tipo, CantidadHabitantes, Precio, Estado, IdPropietario FROM Inmuebles" +
                    $" WHERE IdInmueble=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            CantidadHabitantes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Estado = reader.GetString(6),
                            IdPropietario = reader.GetInt32(7)
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.IdInmueble, Direccion, Uso, Tipo, CantidadHabitantes, Precio, Estado, i.IdPropietario, p.Nombre, p.Apellido" +
                    $" FROM Inmuebles i JOIN Propietarios p ON(i.IdInmueble = p.IdPropietario)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            CantidadHabitantes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Estado = reader.GetString(6),
                            Propietario = new Propietario
                            {
                                IdPropietario = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9)
                            }
                            
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
      