using GestorMensajesInstitucionales.Domain.Entities;
using GestorMensajesInstitucionales.Domain.Enums;

namespace GestorMensajesInstitucionales.Application.Interfaces;

public interface IRegistroService
{
    Task<Registro> CrearRegistroAsync(Registro registro, IEnumerable<int> topicIds, Usuario usuarioActual);
    Task<Registro> ActualizarRegistroAsync(Registro registro, IEnumerable<int> topicIds, Usuario usuarioActual);
    Task EliminarRegistroAsync(Guid registroId, Usuario usuarioActual);
    Task<Registro?> ObtenerPorIdAsync(Guid id);
    Task<IReadOnlyList<Registro>> BuscarAsync(TipoRegistro? tipoRegistro, TipoFlujo? tipoFlujo, string? promotorId, string? ejecutivoId, int? clasificacionId, IEnumerable<int>? topicIds, DateTime? desde, DateTime? hasta, string? textoLibre);
    Task AsociarPdfAsync(Guid registroId, string pdfPath, Usuario usuarioActual);
    Task<Attachment> AgregarAdjuntoAsync(Guid registroId, Attachment adjunto, Stream contenido, Usuario usuarioActual);
    Task RemoverAdjuntoAsync(Guid adjuntoId, Usuario usuarioActual);
    Task<IReadOnlyList<string>> DescomprimirAdjuntoAsync(Guid adjuntoId, string destinoTemporal);
}
