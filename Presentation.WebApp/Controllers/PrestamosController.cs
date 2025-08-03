using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamosDbContext _prestamosDbContext;
        private readonly UsuariosDbContext _usuariosDbContext;
        private readonly LibrosDbContext _librosDbContext;

        public PrestamosController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _prestamosDbContext = new PrestamosDbContext(connectionString);
            _usuariosDbContext = new UsuariosDbContext(connectionString);
            _librosDbContext = new LibrosDbContext(connectionString);
        }

        // Muestra la lista de préstamos
        public IActionResult Index()
        {
            var data = _prestamosDbContext.List();
            return View(data);
        }

        // Detalles de un préstamo específico
        public IActionResult Details(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            if (data == null)
                return NotFound();

            return View(data);
        }

        // Formulario para crear préstamo nuevo
        public IActionResult Create()
        {
            PopulateSelectLists(); // llena dropdowns de usuarios y libros
            return View();
        }

        [HttpPost]
        public IActionResult Create(IM253E08Prestamo data)
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists(data);
                return View(data);
            }

            data.Id = Guid.NewGuid();
            data.FechaPrestamo = DateTime.Now;

            _prestamosDbContext.Create(data);
            return RedirectToAction("Index");
        }

        // Formulario para editar préstamo existente
        public IActionResult Edit(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            if (data == null)
                return NotFound();

            PopulateSelectLists(data);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(IM253E08Prestamo data)
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists(data);
                return View(data);
            }

            _prestamosDbContext.Edit(data);
            return RedirectToAction("Index");
        }

        // Acción para eliminar préstamo
        public IActionResult Delete(Guid id)
        {
            _prestamosDbContext.Delete(id);
            return RedirectToAction("Index");
        }

        // Método privado para poblar dropdowns con usuarios y libros
        private void PopulateSelectLists(IM253E08Prestamo? prestamo = null)
        {
            var usuarios = _usuariosDbContext.List();
            var libros = _librosDbContext.List();

            ViewBag.UsuarioId = new SelectList(usuarios, "Id", "Nombre", prestamo?.UsuarioId);
            ViewBag.LibroId = new SelectList(libros, "Id", "Autor", prestamo?.LibroId);
        }
    }
}