using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoInmobiliariaHugo.Models;

namespace ProyectoInmobiliariaHugo.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly IRepositorio<Propietario> repositorio;
        private readonly IConfiguration config;
        private readonly DataContext context;

        public PropietarioController(IRepositorio<Propietario> repositorio, IConfiguration config, DataContext context)
        {
            this.repositorio = repositorio;
            this.config = config;
            this.context = context;
        }

        [Authorize]
        // GET: Propietario/Index
        public ActionResult Index()
        {
            //Data data = new Data();
           // var propietarios = repositorio.ObtenerTodos();
            //if (TempData.ContainsKey("Id"))
           //     ViewBag.Id = TempData["Id"];

            return View();
        }

        [Authorize(Policy = "Administrador")]
        // GET: Propietario/Listado
        public ActionResult Listado()
        {
            IEnumerable<Propietario> propietario = repositorio.ObtenerTodos();
            
                //context.Propietario.ToList();
            return View(propietario);
        }

        // GET: Propietario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Policy = "Administrador")]
        // GET: Propietario/Create
        public ActionResult Create()
        {
            // ViewBag.Ruta = "Propietario";
            //ViewBag.Query = "Propietario";
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                TempData["Nombre"] = propietario.Nombre;
                if (ModelState.IsValid)
                {
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    repositorio.Alta(propietario);
                    TempData["Id"] = propietario.IdPropietario;
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: Propietario/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                if(propietario.Dni == 0)
                {
                    ViewBag.DniNull = "Ingrese Dni";
                    return View();
                }
                if (propietario.Nombre == null)
                {
                    ViewBag.NombreNull = "Ingrese Nombre";
                    return View();
                }
                if (propietario.Apellido == null)
                {
                    ViewBag.ApellidoNull = "Ingrese Apellido";
                    return View();
                }
                if (propietario.Telefono == null)
                {
                    ViewBag.TelefonoNull = "Ingrese Teléfono";
                    return View();
                }
                if (propietario.Email == null)
                {
                    ViewBag.Email = "Ingrese Email";
                    return View();
                }
                if (propietario.Direccion == null)
                {
                    ViewBag.DireccionNull = "Ingrese Dirección";
                    return View();
                }
                if(propietario.Clave != null)
                {
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["SALADA"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                }
                else
                {
                    Propietario propietario2 = context.Propietarios.First(x => x.IdPropietario == id);
                    propietario.Clave = propietario2.Clave;
                }
                repositorio.Modificacion(propietario);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(propietario);
            }
        }
        /*
        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PropietarioView propietario)
        {
            try
            {

                if (propietario.Dni != 0 && propietario.Nombre != null && propietario.Apellido != null && propietario.Domicilio != null && propietario.Telefono != 0 && propietario.Email != null && propietario.Clave != null)
                {
                    Propietario propietarioedit = contexto.Propietario.First(x => x.Id == propietario.Id);
                    @ViewBag.nomrePropietario = propietarioedit.Nombre + " " + propietarioedit.Apellido;
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                      password: propietario.Clave,
                      salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                      prf: KeyDerivationPrf.HMACSHA1,
                      iterationCount: 1000,
                      numBytesRequested: 256 / 8));
                    p = new Propietario
                    {
                        Id = propietario.Id,
                        Dni = propietario.Dni,
                        Nombre = propietario.Nombre,
                        Apellido = propietario.Apellido,
                        Email = propietario.Email,
                        Telefono = propietario.Telefono,
                        Clave = propietario.Clave,
                        Domicilio = propietario.Domicilio
                    };

                    contexto.Propietario.Update(p);
                    contexto.SaveChanges();
                    return RedirectToAction("ListaPropietarios");
                }
                else
                {
                    Propietario propietarioedit = contexto.Propietario.First(x => x.Id == propietario.Id);
                    @ViewBag.nomrePropietario = propietarioedit.Nombre + " " + propietarioedit.Apellido;
                    ViewBag.Error = "ingrese Todos los datos";
                    return View();
                }


            }
            catch (Exception ex)
            {
                Propietario propietarioedit = contexto.Propietario.First(x => x.Id == propietario.Id);
                @ViewBag.nomrePropietario = propietarioedit.Nombre + " " + propietarioedit.Apellido;
                ViewBag.exception = ex;
                return View();
            }
        }
        */
        // GET: Propietario/Delete/5
        public ActionResult Eliminar(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Propietario entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }
    }
}