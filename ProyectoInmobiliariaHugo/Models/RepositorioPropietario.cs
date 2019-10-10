using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration): base(configuration)
        {

        }

        public int Alta(Propietario p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Propietarios (Dni, Nombre, Apellido, Telefono, Email, Direccion, Clave) " +
                    $"VALUES ({p.Dni},'{p.Nombre}', '{p.Apellido}',{p.Telefono},'{p.Email}','{p.Direccion}','{p.Clave}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdPropietario = Convert.ToInt32(id);
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
                string sql = $"DELETE FROM Propietarios WHERE IdPropietario = {id}";
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

        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Propietarios SET Dni={p.Dni}, Nombre='{p.Nombre}', Apellido='{p.Apellido}', Telefono='{p.Telefono}', Email='{p.Email}', Direccion='{p.Direccion}', Clave='{p.Clave}' " +
                    $"WHERE IdPropietario = {p.IdPropietario}";
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

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Dni, Nombre, Apellido, Telefono, Email, Direccion, Clave" +
                    $" FROM Propietarios";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Apellido = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Direccion = reader.GetString(6),
                            Clave = reader.GetString(7)
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Propietario ObtenerPorId(int id)
        {
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario,Dni,Nombre,Apellido,Telefono,Email,Direccion,Clave FROM Propietarios WHERE IdPropietario=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Apellido = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Direccion = reader.GetString(6),
                            Clave = reader["Clave"].ToString(),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }


        public Propietario ObtenerPorEmail(string email)
        {
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdPropietario, Dni, Nombre, Apellido, Telefono, Email, Direccion, Clave FROM Propietarios" +
                    $" WHERE Email=@email";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Propietario
                        {
                            IdPropietario = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Apellido = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Direccion = reader.GetString(6),
                            Clave = reader.GetString(7),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }
    }
}
