using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class Inmueble
    {
        [Key]
        public int IdInmueble { get; set; }
        public string Direccion { get; set; }
        public string Uso { get; set; }
        public string Tipo { get; set; }
        public int CantidadHabitantes { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
        public int IdPropietario { get; set; }
        public virtual Propietario Propietario { get; set; }

    }
}
