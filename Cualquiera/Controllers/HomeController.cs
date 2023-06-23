using Cualquiera.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace Cualquiera.Controllers
{

    [Authorize(Roles = "Administrador")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClinicaContext _clinica;
        
        public HomeController(ILogger<HomeController> logger, ClinicaContext clinica)
        {
            _logger = logger;
            _clinica = clinica;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //Avanzado en la clase
        public IActionResult GetResult()
        {
            var data = _clinica.Administradors.ToList();
            //new {mensaje =  "hola como estan " };
            return Json(data);
        }
        //En Index.cshtml de home falta codigo a implementar
    }
}
