namespace GestorMensajesInstitucionales.Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; }
    public Guid RegistroId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; // zip, rar
    public long FileSizeBytes { get; set; }
    public string StoredPath { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public Registro? Registro { get; set; }
}
