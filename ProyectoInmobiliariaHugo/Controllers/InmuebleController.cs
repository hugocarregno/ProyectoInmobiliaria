using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Index()
        {
            var inmuebles = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(inmuebles);
            
        }
        // GET: Inmuebles/Create
        //[Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.propietarios = propietarios.ObtenerTodos();
            return View();
        }


        // POST: Inmuebles/Create
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