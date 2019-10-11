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
    public class InmuebleController : Controller
    {
        private readonly IRepositorio<Inmueble> repositorio;
        public IRepositorio<Propietario> propietarios;
        private readonly DataContext context;
        public InmuebleController(IRepositorio<Inmueble> repositorio, IRepositorio<Propietario> propietarios, DataContext context)
        {
            this.repositorio = repositorio;
            this.propietarios = propietarios;
            this.context = context;
        }
        // GET: Inmueble/Index
        public ActionResult Index()
        {

            return RedirectToAction("Listado");

        }
        //GET: Inmueble/Listado
        [Authorize]
        public ActionResult Listado()
        {
            //var inmuebles = repositorio.ObtenerTodos();
            //if (TempData.ContainsKey("Id"))
            //    ViewBag.Id = TempData["Id"];
            //return View(inmuebles);
            var inmueble = context.Inmuebles.Include(c => c.Propietario);
            return View(inmueble.ToList());
        }

        // GET: Inmueble/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(int id)
        {

            if (id == 0)
            {
                ViewBag.IdPropietario = 0;
                ViewBag.Propietarios = propietarios.ObtenerTodos();
                return View();
            }
            //ViewBag.Propietarios = propietarios.ObtenerPorId(id);
            Propietario propietario = propietarios.ObtenerPorId(id);
            ViewBag.IdPropietario = propietario.IdPropietario;
            ViewBag.PropietarioDescripcion = propietario.Dni + " " + propietario.Apellido + " " + propietario.Nombre;
            return View();
        }

        [Authorize(Policy = "Administrador")]
        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                //repositorio.Alta(inmueble);
                //return RedirectToAction(nameof(Index));
                ViewBag.PropietarioId = inmueble.IdPropietario;
                Propietario propietario = context.Propietarios.First(x => x.IdPropietario == inmueble.IdPropietario);
                ViewBag.PropietarioDescripcion = propietario.Dni + "" + propietario.Nombre + " " + propietario.Apellido;
                Inmueble inmuebleAlta = new Inmueble
                {
                    Direccion = inmueble.Direccion,
                    Uso = inmueble.Uso,
                    Tipo = inmueble.Tipo,
                    CantidadHabitantes = inmueble.CantidadHabitantes,
                    Precio = inmueble.Precio,
                    Estado = inmueble.Estado,
                    IdPropietario = inmueble.IdPropietario,

                };

                if (inmueble.Precio == 0)
                {
                    ViewBag.PrecioNulo = "Precio incompleto";
                    return View();
                }
                if (inmueble.Direccion == null)
                {
                    ViewBag.DireccionNulo = "Dirección incompleta";
                    return View();
                }
                if (inmueble.CantidadHabitantes == 0)
                {
                    ViewBag.CantidadHabitantesNulo = "Cantidad Habitantes incompleta";
                    return View();
                }
                context.Inmuebles.Add(inmuebleAlta);
                context.SaveChanges();
                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.propietarios = propietarios.ObtenerTodos();
                return View(inmueble);
            }
        }
        [Authorize(Policy = "Administrador")]
        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            Inmueble inmueble = context.Inmuebles.First(x => x.IdInmueble == id);
            Propietario propietario = context.Propietarios.First(p => p.IdPropietario == inmueble.IdPropietario);

            ViewBag.IdPropietario = propietario.IdPropietario;
            ViewBag.PropietarioDescripcion = propietario.Dni + " " + propietario.Nombre + " " + propietario.Apellido;
            return View(inmueble);
        }

        [Authorize(Policy = "Administrador")]
        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inmueble inmueble)
        {
            try
            {
                if(inmueble.Precio == 0)
                {
                    ViewBag.PrecioNulo = "Precio incompleto";
                }
                if(inmueble.Direccion == null)
                {
                    ViewBag.DireccionNula = "Dirección Incompleta";
                }
                if(inmueble.CantidadHabitantes == 0)
                {
                    ViewBag.CantidadHabitantes = "Cantidad de Habitantes incompleta";
                }
                if (inmueble.Precio == 0 || inmueble.Direccion == null || inmueble.CantidadHabitantes == 0)
                {

                    Propietario propietario = context.Propietarios.First(p => p.IdPropietario == inmueble.IdPropietario);

                    ViewBag.IdPropietario = propietario.IdPropietario;
                    ViewBag.PropietarioDescripcion = propietario.Dni + " " + propietario.Nombre + " " + propietario.Apellido;
                    return View(inmueble);
                }
                context.Inmuebles.Update(inmueble);
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
        // GET: Inmuebles/Delete/5
        public ActionResult Delete(int id)
        {
            Inmueble inmueble = context.Inmuebles.First(x => x.IdInmueble == id);
            Propietario propietario = context.Propietarios.First(p => p.IdPropietario == inmueble.IdPropietario);

            ViewBag.PropietarioDescripcion = propietario.Dni + " " + propietario.Nombre + " " + propietario.Apellido;

            return View(inmueble);
        }

        [Authorize(Policy = "Administrador")]
        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Inmueble inmueble)
        {
            try
            {
                context.Inmuebles.Remove(inmueble);
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