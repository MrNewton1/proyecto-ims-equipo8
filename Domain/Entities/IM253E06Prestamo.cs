using System;

namespace Domain
{
    /// <summary>
    /// Representa un registro de préstamo de un libro asociado a un usuario.
    /// </summary>
    public class IM253E08Prestamo
    {
        /// <summary>
        /// Identificador único del préstamo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Clave foránea al usuario que realiza el préstamo.
        /// </summary>
        public Guid UsuarioId { get; set; }

        /// <summary>
        /// Clave foránea al libro prestado.
        /// </summary>
        public Guid LibroId { get; set; }

        /// <summary>
        /// Fecha en que se realizó el préstamo.
        /// </summary>
        public DateTime FechaPrestamo { get; set; }

        /// <summary>
        /// Fecha en que se devolvió el libro. Nula si aún no ha sido devuelto.
        /// </summary>
        public DateTime? FechaDevolucion { get; set; }

        /// <summary>
        /// Propiedad de navegación al usuario asociado al préstamo.
        /// </summary>
        public IM253E08Usuario? Usuario { get; set; }

        /// <summary>
        /// Propiedad de navegación al libro asociado al préstamo.
        /// </summary>
        public IM253E08Libro? Libro { get; set; }
    }
}
