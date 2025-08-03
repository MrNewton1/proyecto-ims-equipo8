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

        public IActionResult Index()
        {
            var data = _usuariosDbContext.List();
            return View(data);
        }

        public IActionResult Details(Guid id)
        {
            var data = _usuariosDbContext.Details(id);
            if (data == null || data.Id == Guid.Empty)
                return NotFound();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IM253E08Usuario data)
        {
            if (!ModelState.IsValid)
                return View(data);

            data.Id = Guid.NewGuid();
            _usuariosDbContext.Create(data);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            var data = _usuariosDbContext.Details(id);
            if (data == null || data.Id == Guid.Empty)
                return NotFound();
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(IM253E08Usuario data)
        {
            if (!ModelState.IsValid)
                return View(data);

            _usuariosDbContext.Edit(data);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            _usuariosDbContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}