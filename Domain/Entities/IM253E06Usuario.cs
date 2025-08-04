using System;

namespace Domain
{
    /// <summary>
    /// Representa un usuario del sistema con información de contacto.
    /// </summary>
    public class IM253E08Usuario
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Dirección física del usuario. Puede ser nula si no se proporciona.
        /// </summary>
        public string? Direccion { get; set; }

        /// <summary>
        /// Número de teléfono de contacto del usuario.
        /// </summary>
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Correo { get; set; } = string.Empty;
    }
}
