using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers
{
    /// <summary>
    /// Gestiona las operaciones CRUD para la entidad Préstamo,
    /// incluyendo la selección de usuarios y libros desde listas desplegables.
    /// </summary>
    public class PrestamosController : Controller
    {
        private readonly PrestamosDbContext _prestamosDbContext;
        private readonly UsuariosDbContext _usuariosDbContext;
        private readonly LibrosDbContext _librosDbContext;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="PrestamosController"/>,
        /// inyectando los distintos contextos de datos.
        /// </summary>
        /// <param name="configuration">
        /// Proveedor de configuración para obtener la cadena de conexión
        /// desde <c>appsettings.json</c> (DefaultConnection).
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Se lanza si no existe la cadena de conexión <c>DefaultConnection</c>.
        /// </exception>
        public PrestamosController(IConfiguration configuration)
        {
            var connectionString = configuration
                .GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");

            _prestamosDbContext = new PrestamosDbContext(connectionString);
            _usuariosDbContext = new UsuariosDbContext(configuration.GetConnectionString("DefaultConnection")!);
            _librosDbContext = new LibrosDbContext(configuration.GetConnectionString("DefaultConnection")!);
        }

        /// <summary>
        /// Muestra la lista de todos los préstamos registrados.
        /// </summary>
        /// <returns>
        /// La vista <c>Index</c> con un modelo <see cref="List{IM253E08Prestamo}"/>.
        /// </returns>
        public IActionResult Index()
        {
            var data = _prestamosDbContext.List();
            return View(data);
        }

        /// <summary>
        /// Muestra los detalles de un préstamo específico.
        /// </summary>
        /// <param name="id">Identificador único del préstamo.</param>
        /// <returns>
        /// La vista <c>Details</c> con el modelo <see cref="IM253E08Prestamo"/>
        /// o <c>NotFound</c> si no existe.
        /// </returns>
        public IActionResult Details(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            if (data == null)
                return NotFound();

            return View(data);
        }

        /// <summary>
        /// Muestra el formulario para crear un nuevo préstamo.
        /// </summary>
        /// <returns>La vista <c>Create</c> con listas desplegables pobladas.</returns>
        public IActionResult Create()
        {
            PopulateSelectLists();
            return View();
        }

        /// <summary>
        /// Procesa el envío del formulario de creación de préstamo.
        /// </summary>
        /// <param name="data">Datos del nuevo préstamo.</param>
        /// <returns>
        /// Redirige a <c>Index</c> en caso de éxito o vuelve a mostrar
        /// la vista <c>Create</c> con errores de validación.
        /// </returns>
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

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Muestra el formulario para editar un préstamo existente.
        /// </summary>
        /// <param name="id">Identificador único del préstamo a editar.</param>
        /// <returns>
        /// La vista <c>Edit</c> con el modelo cargado o <c>NotFound</c>.
        /// </returns>
        public IActionResult Edit(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            if (data == null)
                return NotFound();

            PopulateSelectLists(data);
            return View(data);
        }

        /// <summary>
        /// Procesa el envío del formulario de edición de préstamo.
        /// </summary>
        /// <param name="data">Datos del préstamo actualizados.</param>
        /// <returns>
        /// Redirige a <c>Index</c> en caso de éxito o vuelve a mostrar
        /// la vista <c>Edit</c> con errores de validación.
        /// </returns>
        [HttpPost]
        public IActionResult Edit(IM253E08Prestamo data)
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists(data);
                return View(data);
            }

            _prestamosDbContext.Edit(data);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Elimina un préstamo por su <paramref name="id"/> y redirige a <c>Index</c>.
        /// </summary>
        /// <param name="id">Identificador único del préstamo a eliminar.</param>
        /// <returns>Redirección a la acción <c>Index</c>.</returns>
        public IActionResult Delete(Guid id)
        {
            _prestamosDbContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Método privado que llena ViewBag con listas desplegables de usuarios y libros.
        /// </summary>
        /// <param name="prestamo">
        /// (Opcional) Préstamo actual para preseleccionar valores en los dropdowns.
        /// </param>
        private void PopulateSelectLists(IM253E08Prestamo? prestamo = null)
        {
            var usuarios = _usuariosDbContext.List();
            var libros = _librosDbContext.List();

            ViewBag.UsuarioId = new SelectList(
                usuarios, "Id", "Nombre", prestamo?.UsuarioId);

            ViewBag.LibroId = new SelectList(
                libros, "Id", "Autor", prestamo?.LibroId);
        }
    }
}
