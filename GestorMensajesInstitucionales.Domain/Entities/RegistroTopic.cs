namespace GestorMensajesInstitucionales.Domain.Entities;

public class RegistroTopic
{
    public Guid RegistroId { get; set; }
    public int TopicId { get; set; }
    public Registro? Registro { get; set; }
    public Topic? Topic { get; set; }
}
