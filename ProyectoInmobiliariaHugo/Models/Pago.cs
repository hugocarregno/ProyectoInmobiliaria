using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int Cuota { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Importe { get; set; }
        public int IdContrato { get; set; }
    }
}
