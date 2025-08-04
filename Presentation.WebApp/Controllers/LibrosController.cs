using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers
{
    /// <summary>
    /// Gestiona las operaciones CRUD y los detalles Ajax de la entidad Libro.
    /// </summary>
    public class LibrosController : Controller
    {
        private readonly LibrosDbContext _librosDbContext;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="LibrosController"/>.
        /// </summary>
        /// <param name="configuration">
        /// Proveedor de configuración para obtener la cadena de conexión
        /// desde <c>appsettings.json</c> (DefaultConnection).
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Se lanza si no existe la cadena de conexión <c>DefaultConnection</c>.
        /// </exception>
        public LibrosController(IConfiguration configuration)
        {
            var connectionString = configuration
                .GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");

            _librosDbContext = new LibrosDbContext(connectionString);
        }

        /// <summary>
        /// Muestra la lista de todos los libros registrados.
        /// </summary>
        /// <returns>
        /// La vista <c>Index</c> con un modelo <see cref="List{IM253E08Libro}"/>.
        /// </returns>
        public IActionResult Index()
        {
            var data = _librosDbContext.List();
            return View(data);
        }

        /// <summary>
        /// Devuelve los detalles de un libro. Si la petición es Ajax,
        /// retorna solo la vista parcial; de lo contrario, la vista completa.
        /// </summary>
        /// <param name="id">Identificador único del libro.</param>
        /// <returns>
        /// <c>PartialView("_DetallesPartial")</c> o <c>View</c> con el modelo <see cref="IM253E08Libro"/>.
        /// </returns>
        public IActionResult Details(Guid id)
        {
            var libro = _librosDbContext.Details(id);
            if (libro == null)
                return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_DetallesPartial", libro);

            return View(libro);
        }

        /// <summary>
        /// Muestra el formulario para crear un nuevo libro.
        /// </summary>
        /// <returns>La vista <c>Create</c> vacía.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Crea un nuevo libro en la base de datos y redirige a <c>Index</c>.
        /// </summary>
        /// <param name="data">Datos del libro a crear.</param>
        /// <returns>Redirección a la acción <c>Index</c>.</returns>
        [HttpPost]
        public IActionResult Create(IM253E08Libro data)
        {
            data.Id = Guid.NewGuid();
            _librosDbContext.Create(data);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Muestra el formulario para editar un libro existente.
        /// </summary>
        /// <param name="id">Identificador único del libro a editar.</param>
        /// <returns>La vista <c>Edit</c> con el modelo cargado.</returns>
        public IActionResult Edit(Guid id)
        {
            var data = _librosDbContext.Details(id);
            return View(data);
        }

        /// <summary>
        /// Aplica los cambios de edición a un libro y redirige a <c>Index</c>.
        /// </summary>
        /// <param name="data">Datos del libro actualizados.</param>
        /// <returns>Redirección a la acción <c>Index</c>.</returns>
        [HttpPost]
        public IActionResult Edit(IM253E08Libro data)
        {
            _librosDbContext.Edit(data);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Elimina un libro por su <paramref name="id"/> y redirige a <c>Index</c>.
        /// </summary>
        /// <param name="id">Identificador único del libro a eliminar.</param>
        /// <returns>Redirección a la acción <c>Index</c>.</returns>
        public IActionResult Delete(Guid id)
        {
            _librosDbContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
