using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliariaHugo.Models;
using Microsoft.Extensions.Configuration;

namespace ProyectoInmobiliariaHugo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioPropietario propietarios;
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public HomeController(IRepositorioPropietario propietarios, DataContext contexto, IConfiguration config)
        {
            this.propietarios = propietarios;
            this.contexto = contexto;
            this.config = config;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Seguro()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return View(claims);
        }

        // GET: Home/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginView loginView)
        {
            try
            {
                if (loginView.Email == null && loginView.Clave == null)
                {
                    ViewBag.EmailNulo = "Ingrese Email";
                    ViewBag.ClaveNula = "Ingrese Clave";
                    return View();
                }
                if(loginView.Email == null)
                {
                    ViewBag.EmailNulo = "Ingrese Email";
                    return View();
                }
                if (loginView.Clave == null)
                {
                    ViewBag.ClaveNula = "Ingrese Clave";
                    return View();
                }
                //user admin@gmail.com pass admin
                //hashed para admin jPX1uaYX24ssdEttdOrpqpsaU7LpXs7rh3jmUcyCRA8=
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                if (loginView.Perfil == "Propietario")
                {
                    var p = propietarios.ObtenerPorEmail(loginView.Email);
                    if(p == null || p.Clave != hashed)
                    {
                        ViewBag.Mensaje = "Datos inválidos";
                        return View();
                    }
                    var claims = new List<Claim>
                    {
                        new Claim("Id", p.IdPropietario+""),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim(ClaimTypes.Role, "Usuario")
                    };
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.
                        AllowRefresh = true,
                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                    return RedirectToAction("Index", "Propietario");
                }
                if (loginView.Perfil == "Administrador") {

                    Administrador administrador = contexto.Administradores.FirstOrDefault(x => x.Email == loginView.Email && x.Clave == hashed);
                    //if(administrador == null || administrador.Clave != hashed)
                    if (administrador == null || administrador.Clave != hashed)
                    {
                        ViewBag.Mensaje = "Datos inválidos";
                        return View();
                    }
                    var claims = new List<Claim>
                    {
                        new Claim("Id", administrador.IdAdministrador+""),
                        new Claim("FullName", administrador.Nombre + " " + administrador.Apellido),
                        new Claim(ClaimTypes.Role, "Administrador")
                    };
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.
                        AllowRefresh = true,
                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                    return RedirectToAction("Listado", "Propietario");
                }
                return null;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Administrador()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return View(claims);
        }

        [Authorize(Policy = "Usuario")]
        public IActionResult Usuario()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return View(claims);
        }
        
        // GET: Home/Logout
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

    }
}