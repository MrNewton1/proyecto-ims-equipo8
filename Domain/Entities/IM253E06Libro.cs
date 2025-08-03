namespace Domain;
public class IM253E08Libro
{

    public Guid Id { get; set; }
    public string Autor { get; set; } = string.Empty;
    public string? Editorial { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string? Foto { get; set; }
}