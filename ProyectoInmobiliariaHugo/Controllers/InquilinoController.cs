using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoInmobiliariaHugo.Models;

namespace ProyectoInmobiliariaHugo.Controllers
{
    public class InquilinoController : Controller
    {

        private readonly DataContext context;
        Inquilino inquilinoAlta;


        public InquilinoController(DataContext context)
        {
            this.context = context;
        }


        [Authorize(Policy = "Administrador")]
        // GET: Inquilino/Listado
        public ActionResult Listado()
        {
            IEnumerable<Inquilino> inquilinos = context.Inquilinos.ToList();
            return View(inquilinos);
        }

        [Authorize(Policy = "Administrador")]
        // GET: Inquilino/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "Administrador")]
        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                
                if (inquilino.Dni != 0 && inquilino.Nombre != null && inquilino.Apellido != null && inquilino.Direccion != null && inquilino.Telefono != null && inquilino.Email != null && inquilino.LugarTrabajo != null)
                {

                    inquilinoAlta = new Inquilino
                    {
                        Dni = inquilino.Dni,
                        Nombre = inquilino.Nombre,
                        Apellido = inquilino.Apellido,
                        Email = inquilino.Email,
                        Telefono = inquilino.Telefono,
                        LugarTrabajo = inquilino.LugarTrabajo,
                        Direccion = inquilino.Direccion
                    };
                    Inquilino InquilinoCheck = context.Inquilinos.FirstOrDefault(x => x.Dni == inquilinoAlta.Dni);
                    if (InquilinoCheck != null)
                    {
                        ViewBag.registrado = "ya existe un Propietario con ese email o dni";
                        return View();
                    }
                    context.Inquilinos.Add(inquilinoAlta);
                    context.SaveChanges();
                    int res = inquilinoAlta.IdInquilino;
                    if (res != 0)
                    {

                        return RedirectToAction("Listado");
                    }
                    else
                    {
                        ViewBag.registrado = "Error al insertar";
                        return View();
                    }

                }
                else
                {
                    ViewBag.registrado = "Complete todos campos";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        [Authorize(Policy = "Administrador")]
        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id)
        {
            Inquilino inquilino = context.Inquilinos.First(x => x.IdInquilino == id);
            return View(inquilino);

        }

        [Authorize(Policy = "Administrador")]
        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inquilino inquilino)
        {
            try
            {
                if (inquilino.Dni != 0 && inquilino.Nombre != null && inquilino.Apellido != null && inquilino.Direccion != null && inquilino.Telefono != null && inquilino.Email != null && inquilino.LugarTrabajo != null)
                {

                    inquilinoAlta = new Inquilino
                    {
                        IdInquilino = inquilino.IdInquilino,
                        Dni = inquilino.Dni,
                        Nombre = inquilino.Nombre,
                        Apellido = inquilino.Apellido,
                        Email = inquilino.Email,
                        Telefono = inquilino.Telefono,
                        LugarTrabajo = inquilino.LugarTrabajo,
                        Direccion = inquilino.Direccion
                    };
                    context.Inquilinos.Update(inquilinoAlta);
                    context.SaveChanges();
                    return RedirectToAction("Listado");
                }
                else
                {
                    ViewBag.error = "Ingrese Todos los datos";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex;
                return View();
            }
        }

        [Authorize(Policy = "Administrador")]
        // GET: Inquilino/Delete/5
        public ActionResult Delete(int id)
        {
            Inquilino inquilino = context.Inquilinos.First(x => x.IdInquilino == id);
            return View(inquilino);
        }

        [Authorize(Policy = "Administrador")]
        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Inquilino inquilino)
        {
            try
            {
                Inquilino inquilinoDelete = context.Inquilinos.First(x => x.IdInquilino == inquilino.IdInquilino);

                context.Inquilinos.Remove(inquilinoDelete);
                context.SaveChanges();
                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex;
                return View();
            }
        }
    }
}