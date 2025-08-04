namespace Presentation.WebApp.Models
{
    /// <summary>
    /// Representa el modelo de datos que se envía a la vista de error,
    /// incluyendo información opcional para mostrar el identificador de la solicitud.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Obtiene o establece el identificador de la solicitud actual,
        /// útil para rastrear errores en los logs o en el diagnóstico.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indica si se debe mostrar el <see cref="RequestId"/> en la vista.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
