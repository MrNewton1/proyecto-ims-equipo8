using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.WebApp.Models;
using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers
{
    /// <summary>
    /// Controlador principal que maneja la página de inicio, la privacidad y la gestión de errores.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly LibrosDbContext _librosDbContext;
        private readonly UsuariosDbContext _usuariosDbContext;
        private readonly PrestamosDbContext _prestamosDbContext;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Constructor que inyecta los contextos de datos y el logger.
        /// </summary>
        /// <param name="librosDbContext">Contexto para operaciones sobre libros.</param>
        /// <param name="usuariosDbContext">Contexto para operaciones sobre usuarios.</param>
        /// <param name="prestamosDbContext">Contexto para operaciones sobre préstamos.</param>
        /// <param name="logger">Logger para registrar información y errores.</param>
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

        /// <summary>
        /// Muestra la vista de inicio con un gráfico de totales de usuarios, libros y préstamos.
        /// </summary>
        /// <returns>La vista Index con los datos en ViewBag.</returns>
        public IActionResult Index()
        {
            int totalLibros = _librosDbContext.List().Count;
            int totalUsuarios = _usuariosDbContext.List().Count;
            int totalPrestamos = _prestamosDbContext.List().Count;

            ViewBag.Labels = new string[] { "Usuarios", "Libros", "Préstamos" };
            ViewBag.Data = new int[] { totalUsuarios, totalLibros, totalPrestamos };

            return View();
        }

        /// <summary>
        /// Muestra la política de privacidad de la aplicación.
        /// </summary>
        /// <returns>La vista Privacy.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Muestra una página de error genérica cuando ocurre una excepción.
        /// </summary>
        /// <remarks>
        /// Usa ResponseCache para no almacenar esta respuesta.
        /// </remarks>
        /// <returns>La vista Error con información del RequestId.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
