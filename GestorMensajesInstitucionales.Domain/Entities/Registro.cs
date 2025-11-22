using GestorMensajesInstitucionales.Domain.Enums;

namespace GestorMensajesInstitucionales.Domain.Entities;

public class Registro
{
    public Guid Id { get; set; }
    public TipoRegistro TipoRegistro { get; set; }
    public TipoFlujo TipoFlujo { get; set; }
    public string PromotorId { get; set; } = string.Empty;
    public string EjecutivoId { get; set; } = string.Empty;
    public int ClasificacionSeguridadId { get; set; }
    public string Asunto { get; set; } = string.Empty;
    public string? Comentario { get; set; }
    public bool TieneFechaTermino { get; set; }
    public DateTime? FechaTermino { get; set; }
    public string? PdfPath { get; set; }
    public Guid? PredecesorId { get; set; }
    public Registro? Predecesor { get; set; }
    public ICollection<Registro> Respuestas { get; set; } = new List<Registro>();
    public ICollection<Attachment> Adjuntos { get; set; } = new List<Attachment>();
    public ICollection<RegistroTopic> RegistroTopics { get; set; } = new List<RegistroTopic>();
    public Promotor? Promotor { get; set; }
    public Ejecutivo? Ejecutivo { get; set; }
    public ClasificacionSeguridad? ClasificacionSeguridad { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
}
