using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class Administrador
    {
        [Key]
        public int IdAdministrador { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Clave { get; set; }
    }
}
