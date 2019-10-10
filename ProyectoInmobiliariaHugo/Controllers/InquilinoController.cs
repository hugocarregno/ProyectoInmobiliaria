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

        private readonly RepositorioInquilino repositorio;
        private readonly IConfiguration config;

        public InquilinoController(RepositorioInquilino repositorio, IConfiguration config)
        {
            this.repositorio = repositorio;
            this.config = config;
        }

        [Authorize]
        // GET: Inquilino
        public IActionResult Index()
        {
            var inquilinos = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(inquilinos);
        }
    }
}