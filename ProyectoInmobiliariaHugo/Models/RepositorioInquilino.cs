using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class RepositorioInquilino : RepositorioBase, IRepositorio<Inquilino>
    {
        public RepositorioInquilino(IConfiguration configuration): base(configuration)
        {

        }

        public int Alta(Inquilino inquilino)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inquilinos (Dni, Nombre, Apellido, Telefono, Email, Direccion) " +
                    $"VALUES ({inquilino.Dni},'{inquilino.Nombre}','{inquilino.Apellido}',{inquilino.Telefono},'{inquilino.Email}','{inquilino.Direccion}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    inquilino.IdInquilino = (int)command.ExecuteScalar();
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
                string sql = $"DELETE FROM Inquilinos WHERE IdPropietario = {id}";
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

        public int Modificacion(Inquilino inquilino)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inquilinos SET Dni{inquilino.Dni}, Nombre='{inquilino.Nombre}', Apellido='{inquilino.Apellido}', Telefono{inquilino.Telefono}, Email'{inquilino.Email}', Direccion'{inquilino.Direccion}'" +
                    $"WHERE IdInquilino = {inquilino.IdInquilino}";
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

        public Inquilino ObtenerPorId(int id)
        {
            Inquilino inquilino = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInquilino, Dni, Nombre, Apellido, Telefono, Email, Direccion FROM Inquilinos" +
                    $" WHERE IdInquilino=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        inquilino = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Apellido = reader.GetString(3),                            
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Direccion = reader.GetString(6)
                        };
                        return inquilino;
                    }
                    connection.Close();
                }
            }
            return inquilino;
        }

        public IList<Inquilino> ObtenerTodos()
        {
            IList<Inquilino> res = new List<Inquilino>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdInquilino, Dni, Nombre, Apellido, Telefono, Email, Direccion" +
                    $" FROM Inquilinos";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inquilino p = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Apellido = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Direccion = reader.GetString(6)
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
