namespace GestorMensajesInstitucionales.Domain.Entities;

public class Topic
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool EsProyecto { get; set; }
    public ICollection<RegistroTopic> RegistroTopics { get; set; } = new List<RegistroTopic>();
}
