using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class Propietario
    {
        [Key]
        public int IdPropietario { get; set; }
        public int Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Direccion { get; set; }
        [DataType(DataType.Password)]
        public string Clave { get; set; }
        public Boolean Estado { get; set; }
    }
}
