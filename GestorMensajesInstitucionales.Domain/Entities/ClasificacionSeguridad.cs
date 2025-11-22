namespace GestorMensajesInstitucionales.Domain.Entities;

public class ClasificacionSeguridad
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public ICollection<Registro> Registros { get; set; } = new List<Registro>();
}
