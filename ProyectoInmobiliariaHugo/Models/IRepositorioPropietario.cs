using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        Propietario ObtenerPorEmail(string email);
    }
}
