using Cualquiera.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cualquiera.Controllers
{
    public class Account : Controller
    {
        private readonly ClinicaContext _DbContext;
        string mensaje;
        public Account(ClinicaContext context)
        {
            _DbContext = context;
        }
        public IActionResult Index()
        {
            return View("Login");
        }
        [HttpPost]
        public IActionResult Login(Administrador model)
        {
            if(ModelState.IsValid)
            {
                var user = _DbContext.Administradors.FirstOrDefault(u=>u.Rut == model.Rut && u.Password==model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.Rut)
                    };
                    var identity= new ClaimsIdentity(claims,"login");
                    var principal= new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(principal).Wait();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    mensaje = "Error de usuario y/o contraseña";
                    ViewData["Mensaje"] = mensaje;
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Realizar log out
            await HttpContext.SignOutAsync();

            // Limpiar el contexto de base de datos si es necesario
            _DbContext.Dispose();


            // Redirigir a la página de inicio u otra página después del log out
            return RedirectToAction("Index", "Account");
        }

    }
}
