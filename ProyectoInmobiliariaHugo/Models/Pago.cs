using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public int Cuota { get; set; }
        [Display(Name = "Fecha Pago")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaPago { get; set; }
        public decimal Importe { get; set; }
        public int IdContrato { get; set; }
        public virtual Contrato Contrato { get; set; }
    }
}
