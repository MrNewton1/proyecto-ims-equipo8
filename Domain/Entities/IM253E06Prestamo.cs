namespace Domain;
public class IM253E08Prestamo
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid LibroId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }

    // Propiedades de navegación opcionales
    public IM253E08Usuario? Usuario { get; set; }
    public IM253E08Libro? Libro { get; set; }
}