using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class Data
    {
        public List<Propietario> ObtenerPropietarios()
        {
            List<Propietario> res = new List<Propietario>();
            SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BDInmobiliaria;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            String sql = "SELECT IdPropietario, Dni, Apellido, Nombre, Email, Clave FROM Propietarios;";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            var reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Propietario p = new Propietario
                {
                    IdPropietario = reader.GetInt32(0),
                    Dni = reader.GetInt32(1),
                    Apellido = reader["Apellido"].ToString(),
                    Nombre = reader["Nombre"].ToString(),
                    Email = reader[nameof(p.Email)].ToString(),
                    Clave = reader[nameof(p.Clave)].ToString()
                };
                res.Add(p);
            }
            conn.Close();
            return res;
        }
    }
}
