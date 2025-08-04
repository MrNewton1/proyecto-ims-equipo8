using System;

namespace Domain
{
    /// <summary>
    /// Representa un libro en el catálogo del sistema, con información de autor, ISBN y portada.
    /// </summary>
    public class IM253E08Libro
    {
        /// <summary>
        /// Identificador único del libro.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del autor o autores del libro.
        /// </summary>
        public string Autor { get; set; } = string.Empty;

        /// <summary>
        /// Editorial que publicó el libro. Puede ser nula si no se especificó.
        /// </summary>
        public string? Editorial { get; set; }

        /// <summary>
        /// Código ISBN del libro.
        /// </summary>
        public string ISBN { get; set; } = string.Empty;

        /// <summary>
        /// URL o ruta de la imagen de portada del libro. Nula si no hay imagen.
        /// </summary>
        public string? Foto { get; set; }
    }
}
