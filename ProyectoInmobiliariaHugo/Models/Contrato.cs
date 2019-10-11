using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaCierre { get; set; }
        public decimal Monto { get; set; }
        public int IdInmueble { get; set; }
        public int IdInquilino { get; set; }
        public virtual Inmueble Inmueble { get; set; }
        public virtual Inquilino Inquilino { get; set; }
    }
}
