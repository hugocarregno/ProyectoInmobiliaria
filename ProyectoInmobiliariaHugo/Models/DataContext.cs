using Microsoft.EntityFrameworkCore;
using ProyectoInmobiliariaHugo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaHugo.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Inquilino> Inquilinos { get; set; }
    }
}
