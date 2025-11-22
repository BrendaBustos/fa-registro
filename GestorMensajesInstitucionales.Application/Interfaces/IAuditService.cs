using GestorMensajesInstitucionales.Domain.Entities;

namespace GestorMensajesInstitucionales.Application.Interfaces;

public interface IAuditService
{
    Task RegistrarAsync(int usuarioId, string accion, string entidad, string entidadId, string detalle);
    Task<IReadOnlyList<AuditLog>> BuscarAsync(int? usuarioId, DateTime? desde, DateTime? hasta, string? entidad, string? accion);
}
