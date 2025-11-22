using GestorMensajesInstitucionales.Application.Interfaces;
using GestorMensajesInstitucionales.Domain.Entities;
using GestorMensajesInstitucionales.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestorMensajesInstitucionales.Infrastructure.Services;

public class CatalogoService : ICatalogoService
{
    private readonly AppDbContext _context;
    private readonly IAuditService _auditService;

    public CatalogoService(AppDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<IEnumerable<Promotor>> ListarPromotoresAsync() => await _context.Promotores.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Ejecutivo>> ListarEjecutivosAsync() => await _context.Ejecutivos.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<ClasificacionSeguridad>> ListarClasificacionesAsync() => await _context.Clasificaciones.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Topic>> ListarTopicsAsync() => await _context.Topics.AsNoTracking().ToListAsync();

    public async Task<Topic> CrearProyectoAsync(string nombreProyecto, Usuario actor)
    {
        var topic = new Topic { Nombre = $"Proyecto: {nombreProyecto}", EsProyecto = true };
        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(actor.Id, "CREAR", nameof(Topic), topic.Id.ToString(), $"Alta de tema proyecto {topic.Nombre}");
        return topic;
    }
}
