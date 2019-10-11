using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class Inquilino
    {
        [Key]
        public int IdInquilino { get; set; }
        //[Required]
        public int Dni { get; set; }
        //[Required]
        public string Nombre { get; set; }
        //[Required]
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string LugarTrabajo { get; set; }
        
    }
}
