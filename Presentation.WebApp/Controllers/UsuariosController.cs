using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers
{
    /// <summary>
    /// Gestiona las operaciones CRUD y la visualización Ajax para la entidad Usuario.
    /// </summary>
    public class UsuariosController : Controller
    {
        private readonly UsuariosDbContext _usuariosDbContext;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="UsuariosController"/>,
        /// inyectando el contexto de datos de usuarios.
        /// </summary>
        /// <param name="configuration">
        /// Proveedor de configuración para obtener la cadena de conexión
        /// desde <c>appsettings.json</c> (DefaultConnection).
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Se lanza si la cadena de conexión <c>DefaultConnection</c> no está configurada.
        /// </exception>
        public UsuariosController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new ArgumentNullException(
                                       nameof(configuration),
                                       "DefaultConnection no está configurada");
            _usuariosDbContext = new UsuariosDbContext(connectionString);
        }

        /// <summary>
        /// Obtiene y muestra la lista de todos los usuarios registrados.
        /// </summary>
        /// <returns>
        /// La vista <c>Index</c> con un modelo <see cref="List{IM253E08Usuario}"/>.
        /// </returns>
        public IActionResult Index()
        {
            var data = _usuariosDbContext.List();
            return View(data);
        }

        /// <summary>
        /// Devuelve los detalles de un usuario. Si la petición es Ajax,
        /// retorna sólo la vista parcial; de lo contrario, la vista completa.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>
        /// <c>PartialView("_DetallesPartial")</c> o <c>View</c> con el modelo <see cref="IM253E08Usuario"/>.
        /// </returns>
        public IActionResult Details(Guid id)
        {
            var usuario = _usuariosDbContext.Details(id);
            if (usuario == null)
                return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_DetallesPartial", usuario);

            return View(usuario);
        }

        /// <summary>
        /// Muestra el formulario para crear un nuevo usuario.
        /// </summary>
        /// <returns>La vista <c>Create</c> vacía.</returns>
        public IActionResult Create() => View();

        /// <summary>
        /// Procesa el envío del formulario de creación de usuario.
        /// </summary>
        /// <param name="data">Datos del usuario a crear.</param>
        /// <returns>
        /// Redirige a <c>Index</c> en caso de éxito o vuelve a mostrar
        /// la vista <c>Create</c> con errores de validación.
        /// </returns>
        [HttpPost]
        public IActionResult Create(IM253E08Usuario data)
        {
            if (!ModelState.IsValid)
                return View(data);

            data.Id = Guid.NewGuid();
            _usuariosDbContext.Create(data);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Muestra el formulario para editar un usuario existente.
        /// </summary>
        /// <param name="id">Identificador único del usuario a editar.</param>
        /// <returns>
        /// La vista <c>Edit</c> con el modelo cargado o <c>NotFound</c> si no existe.
        /// </returns>
        public IActionResult Edit(Guid id)
        {
            var data = _usuariosDbContext.Details(id);
            if (data == null || data.Id == Guid.Empty)
                return NotFound();

            return View(data);
        }

        /// <summary>
        /// Procesa el envío del formulario de edición de usuario.
        /// </summary>
        /// <param name="data">Datos del usuario actualizados.</param>
        /// <returns>
        /// Redirige a <c>Index</c> en caso de éxito o vuelve a mostrar
        /// la vista <c>Edit</c> con errores de validación.
        /// </returns>
        [HttpPost]
        public IActionResult Edit(IM253E08Usuario data)
        {
            if (!ModelState.IsValid)
                return View(data);

            _usuariosDbContext.Edit(data);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Elimina un usuario por su <paramref name="id"/> y redirige a <c>Index</c>.
        /// </summary>
        /// <param name="id">Identificador único del usuario a eliminar.</param>
        /// <returns>Redirección a la acción <c>Index</c>.</returns>
        public IActionResult Delete(Guid id)
        {
            _usuariosDbContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
