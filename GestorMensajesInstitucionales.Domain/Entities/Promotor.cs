namespace GestorMensajesInstitucionales.Domain.Entities;

public class Promotor
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public ICollection<Registro> Registros { get; set; } = new List<Registro>();
}
