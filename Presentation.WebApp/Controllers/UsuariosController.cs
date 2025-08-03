using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuariosDbContext _usuariosDbContext;

        public UsuariosController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new ArgumentNullException("DefaultConnection no está configurada");
            _usuariosDbContext = new UsuariosDbContext(connectionString);
        }

        // GET: /Usuarios
        public IActionResult Index()
        {
            var data = _usuariosDbContext.List();
            return View(data);
        }

        // GET: /Usuarios/Details/{id}
        public IActionResult Details(Guid id)
        {
            var usuario = _usuariosDbContext.Details(id);
            if (usuario == null)
                return NotFound();

            // Si es petición Ajax, devolvemos sólo la partial
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_DetallesPartial", usuario);

            // Si no, vista completa
            return View(usuario);
        }

        // GET: /Usuarios/Create
        public IActionResult Create() => View();

        // POST: /Usuarios/Create
        [HttpPost]
        public IActionResult Create(IM253E08Usuario data)
        {
            if (!ModelState.IsValid)
                return View(data);

            data.Id = Guid.NewGuid();
            _usuariosDbContext.Create(data);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Usuarios/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            var data = _usuariosDbContext.Details(id);
            if (data == null || data.Id == Guid.Empty)
                return NotFound();
            return View(data);
        }

        // POST: /Usuarios/Edit
        [HttpPost]
        public IActionResult Edit(IM253E08Usuario data)
        {
            if (!ModelState.IsValid)
                return View(data);

            _usuariosDbContext.Edit(data);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Usuarios/Delete/{id}
        public IActionResult Delete(Guid id)
        {
            _usuariosDbContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
