namespace GestorMensajesInstitucionales.Domain.Entities;

public class AuditLog
{
    public long Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime FechaHora { get; set; }
    public string Accion { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public string EntidadId { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public Usuario? Usuario { get; set; }
}
