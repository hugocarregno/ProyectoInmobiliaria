using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInmobiliariaHugo.Models;

namespace ProyectoInmobiliariaHugo.Controllers
{
    public class ContratoController : Controller
    {
        private readonly DataContext context;
        public ContratoController(DataContext context)
        {
            this.context = context;
        }

        [Authorize(Policy = "Administrador")]
        // GET: Contrato/Listado
        public ActionResult Listado()
        {

            var Contratos = context.Contratos.Include(x => x.Inquilino).Include(x => x.Inmueble.Propietario);
            return View(Contratos.ToList());
        }

        [Authorize(Policy = "Administrador")]
        // GET: Contratos/Create
        public ActionResult Create(int id)
        {
            Inmueble inmueble = context.Inmuebles.Include(x => x.Propietario).First(i => i.IdInmueble == id);

            ViewBag.IdInmueble = inmueble.IdInmueble;
            ViewBag.Inmueble = inmueble.Direccion;
            //ViewBag.NombrePropietario = inmueble.Propietario.Nombre + " " + inmueble.Propietario.Apellido;
            return View();
        }

        [Authorize(Policy = "Administrador")]
        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                Inmueble inmueble = context.Inmuebles.First(i => i.IdInmueble == contrato.IdInmueble);

                ViewBag.IdInmueble = inmueble.IdInmueble;
                ViewBag.Inmueble = inmueble.Direccion;

                if (inmueble.Estado == "ocupado")
                {
                    ViewBag.Error = "Inmueble ocupado";
                    return View();
                }

                if (contrato.IdInquilino == 0)
                {
                    ViewBag.Error = "No se encuentra inquilino";
                    return View();
                }

                if(contrato.FechaInicio == null)
                {
                    ViewBag.Error = "Fecha inicio incompleta";
                    return View();
                }

                if (contrato.FechaCierre == null)
                {
                    ViewBag.Error = "Fecha Cierre incompleta";
                    return View();
                }

                if (contrato.Monto == 0)
                {
                    ViewBag.Error = "Monto incompleto";
                    return View();
                }
                Inquilino inquilino = context.Inquilinos.FirstOrDefault(x => x.Dni == contrato.IdInquilino);
                if (inquilino == null)
                {
                    ViewBag.Error = "Inquilino no registrado";
                    return View();
                }
                inmueble.Estado = "ocupado";

                context.Inmuebles.Update(inmueble);
                context.SaveChanges();

                Contrato contratoAlta = new Contrato
                {
                    FechaInicio = contrato.FechaInicio,
                    FechaCierre = contrato.FechaCierre,
                    Monto = contrato.Monto,
                    IdInmueble = contrato.IdInmueble,
                    IdInquilino = inquilino.IdInquilino,
                };

                context.Contratos.Add(contratoAlta);
                context.SaveChanges();

                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.error = ex;
                return View();
            }
        }


    }
}