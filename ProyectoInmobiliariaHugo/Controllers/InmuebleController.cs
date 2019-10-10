using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliariaHugo.Models;

namespace ProyectoInmobiliariaHugo.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IRepositorio<Inmueble> repositorio;
        public IRepositorio<Propietario> propietarios;
        public InmuebleController(IRepositorio<Inmueble> repositorio, IRepositorio<Propietario> propietarios)
        {
            this.repositorio = repositorio;
            this.propietarios = propietarios;
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
            var inmuebles = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(inmuebles);

        }

        // GET: Inmueble/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(int id)
        {
            
            if(id == 0)
            {
                ViewBag.IdPropietario = 0;
                ViewBag.Propietarios = propietarios.ObtenerTodos();
                return View();
            }
            Propietario propietario = propietarios.ObtenerPorId(id);
            ViewBag.IdPropietario = propietario.IdPropietario;
            ViewBag.PropietarioDescripcion = propietario.Dni + " " + propietario.Apellido + " " + propietario.Nombre;
            return View();
        }


        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                repositorio.Alta(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.propietarios = propietarios.ObtenerTodos();
                return View(inmueble);
            }
        }
    }
}