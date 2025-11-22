using GestorMensajesInstitucionales.Application.Interfaces;
using GestorMensajesInstitucionales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GestorMensajesInstitucionales.Infrastructure.Data;

namespace GestorMensajesInstitucionales.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;

    public AuditService(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(int usuarioId, string accion, string entidad, string entidadId, string detalle)
    {
        var audit = new AuditLog
        {
            UsuarioId = usuarioId,
            Accion = accion,
            Entidad = entidad,
            EntidadId = entidadId,
            Detalle = detalle,
            FechaHora = DateTime.UtcNow
        };
        _context.AuditLogs.Add(audit);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<AuditLog>> BuscarAsync(int? usuarioId, DateTime? desde, DateTime? hasta, string? entidad, string? accion)
    {
        var query = _context.AuditLogs.AsNoTracking().AsQueryable();
        if (usuarioId.HasValue)
        {
            query = query.Where(a => a.UsuarioId == usuarioId.Value);
        }
        if (desde.HasValue)
        {
            query = query.Where(a => a.FechaHora >= desde.Value);
        }
        if (hasta.HasValue)
        {
            query = query.Where(a => a.FechaHora <= hasta.Value);
        }
        if (!string.IsNullOrWhiteSpace(entidad))
        {
            query = query.Where(a => a.Entidad == entidad);
        }
        if (!string.IsNullOrWhiteSpace(accion))
        {
            query = query.Where(a => a.Accion == accion);
        }

        return await query.OrderByDescending(a => a.FechaHora).ToListAsync();
    }
}
