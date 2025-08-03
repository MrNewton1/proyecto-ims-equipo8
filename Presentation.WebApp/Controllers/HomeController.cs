using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.WebApp.Models;
using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibrosDbContext _librosDbContext;
        private readonly UsuariosDbContext _usuariosDbContext;
        private readonly PrestamosDbContext _prestamosDbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            LibrosDbContext librosDbContext,
            UsuariosDbContext usuariosDbContext,
            PrestamosDbContext prestamosDbContext,
            ILogger<HomeController> logger)
        {
            _librosDbContext = librosDbContext;
            _usuariosDbContext = usuariosDbContext;
            _prestamosDbContext = prestamosDbContext;
            _logger = logger;
        }

        // Acción principal que muestra totales para la vista inicio con gráfico
        public IActionResult Index()
        {
            int totalLibros = _librosDbContext.List().Count;
            int totalUsuarios = _usuariosDbContext.List().Count;
            int totalPrestamos = _prestamosDbContext.List().Count;

            ViewBag.Labels = new string[] { "Usuarios", "Libros", "Préstamos" };
            ViewBag.Data = new int[] { totalUsuarios, totalLibros, totalPrestamos };

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
    }
}
