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
    public class PagoController : Controller
    {
        private readonly DataContext context;
        public PagoController(DataContext context)
        {
            this.context = context;
        }

        [Authorize(Policy = "Administrador")]
        // GET: Pago/Index
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Administrador")]
        // GET: Pago/Listado
        public ActionResult Listado()
        {
            
                var pago = context.Pagos.Include(x => x.Contrato.Inquilino).Include(x => x.Contrato.Inmueble);
                return View(pago);

        }

        [Authorize(Policy = "Administrador")]
        // GET: Pago/ListaPagosPorContrato
        public ActionResult ListaPagosPorContrato(int id)
        {
            Contrato contrato = context.Contratos.Include(x => x.Inquilino).First(x => x.IdContrato == id);

            ViewBag.Inquilino = contrato.Inquilino.Apellido + " " + contrato.Inquilino.Nombre;

            var pago = context.Pagos.Include(x => x.Contrato.Inquilino).Include(x => x.Contrato.Inmueble).Where(x => x.IdContrato == id);


            return View(pago);
        }

        // GET: Pago/Create
        public ActionResult Create(int id)
        {
            Contrato contrato = context.Contratos.Include(x => x.Inquilino).First(x => x.IdContrato == id);
            ViewBag.IdContrato = contrato.IdContrato;
            ViewBag.InquilinoDescripcion = contrato.Inquilino.Apellido+" "+contrato.Inquilino.Nombre;
            return View();
        }
        
        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                Contrato contrato = context.Contratos.Include(x => x.Inquilino).First(x => x.IdContrato == pago.IdContrato);
                ViewBag.IdContrato = contrato.IdContrato;
                ViewBag.InquilinoDescripcion = contrato.Inquilino.Apellido + " " + contrato.Inquilino.Nombre;


                if (pago.FechaPago == null || pago.Importe == 0)
                {
                    ViewBag.error = "ingrese todos los datos";
                    return View();
                }
                List<Pago> pagos = context.Pagos.Where(x => x.IdContrato == pago.IdContrato).ToList();
                int cuota = pagos.Count();

                Pago pagoAlta = new Pago
                {
                    IdContrato = pago.IdContrato,
                    Cuota = cuota + 1,
                    Importe = pago.Importe,
                    FechaPago = pago.FechaPago,
                    Estado = pago.Estado
                };
                context.Pagos.Add(pagoAlta);
                context.SaveChanges();

                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.error = ex;
                return View();
            }
        }

        
        [Authorize(Policy = "Administrador")]
        // GET: Pago/Edit/5
        public ActionResult Edit(int id)
        {
            Pago pago = context.Pagos.Include(x => x.Contrato.Inquilino).First(x => x.IdPago == id);
            ViewBag.IdContrato = pago.IdContrato;
            ViewBag.Cuota = pago.Cuota;
            ViewBag.InquilinoDescripcion = pago.Contrato.Inquilino.Apellido + " " + pago.Contrato.Inquilino.Nombre;
            ViewBag.FechaPago = pago.FechaPago.ToString("yyyy-MM-dd");
            return View(pago);
        }
        [Authorize(Policy = "Administrador")]
        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pago pago)
        {
            try
            {

                if (pago.FechaPago == null || pago.Importe == 0)
                {
                    Pago pa = context.Pagos.Include(x => x.Contrato.Inquilino).First(x => x.IdPago == pago.IdPago);
                    ViewBag.IdContrato = pago.IdContrato;
                    ViewBag.InquilinoDescripcion = pa.Contrato.Inquilino.Apellido + " " + pa.Contrato.Inquilino.Nombre;
                    ViewBag.FechaPago = pago.FechaPago.ToString("yyyy-MM-dd");
                    ViewBag.error = "Campos Incompletos";
                    return View();
                }

                context.Pagos.Update(pago);
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