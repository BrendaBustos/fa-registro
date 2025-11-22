using GestorMensajesInstitucionales.Application.Interfaces;
using GestorMensajesInstitucionales.Domain.Entities;
using GestorMensajesInstitucionales.Domain.Enums;
using GestorMensajesInstitucionales.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestorMensajesInstitucionales.Infrastructure.Services;

public class RegistroService : IRegistroService
{
    private readonly AppDbContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly IAuditService _auditService;

    public RegistroService(AppDbContext context, IFileStorage fileStorage, IAuditService auditService)
    {
        _context = context;
        _fileStorage = fileStorage;
        _auditService = auditService;
    }

    public async Task<Registro> CrearRegistroAsync(Registro registro, IEnumerable<int> topicIds, Usuario usuarioActual)
    {
        registro.Id = Guid.NewGuid();
        registro.FechaCreacion = DateTime.UtcNow;
        registro.FechaActualizacion = DateTime.UtcNow;
        _context.Registros.Add(registro);
        await ActualizarTopicsAsync(registro, topicIds);
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(usuarioActual.Id, "CREAR", nameof(Registro), registro.Id.ToString(), "Alta de registro");
        return registro;
    }

    public async Task<Registro> ActualizarRegistroAsync(Registro registro, IEnumerable<int> topicIds, Usuario usuarioActual)
    {
        registro.FechaActualizacion = DateTime.UtcNow;
        _context.Registros.Update(registro);
        await ActualizarTopicsAsync(registro, topicIds);
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(usuarioActual.Id, "ACTUALIZAR", nameof(Registro), registro.Id.ToString(), "Edición de registro");
        return registro;
    }

    public async Task EliminarRegistroAsync(Guid registroId, Usuario usuarioActual)
    {
        var registro = await _context.Registros.Include(r => r.Adjuntos).SingleOrDefaultAsync(r => r.Id == registroId) ?? throw new InvalidOperationException("Registro no encontrado");
        foreach (var adjunto in registro.Adjuntos)
        {
            await _fileStorage.DeleteAsync(adjunto.StoredPath);
        }
        _context.Registros.Remove(registro);
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(usuarioActual.Id, "ELIMINAR", nameof(Registro), registroId.ToString(), "Baja de registro");
    }

    public async Task<Registro?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.Registros
            .Include(r => r.Adjuntos)
            .Include(r => r.RegistroTopics).ThenInclude(rt => rt.Topic)
            .Include(r => r.Promotor)
            .Include(r => r.Ejecutivo)
            .Include(r => r.ClasificacionSeguridad)
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IReadOnlyList<Registro>> BuscarAsync(TipoRegistro? tipoRegistro, TipoFlujo? tipoFlujo, string? promotorId, string? ejecutivoId, int? clasificacionId, IEnumerable<int>? topicIds, DateTime? desde, DateTime? hasta, string? textoLibre)
    {
        var query = _context.Registros
            .Include(r => r.RegistroTopics)
            .AsNoTracking()
            .AsQueryable();

        if (tipoRegistro.HasValue)
        {
            query = query.Where(r => r.TipoRegistro == tipoRegistro.Value);
        }
        if (tipoFlujo.HasValue)
        {
            query = query.Where(r => r.TipoFlujo == tipoFlujo.Value);
        }
        if (!string.IsNullOrWhiteSpace(promotorId))
        {
            query = query.Where(r => r.PromotorId == promotorId);
        }
        if (!string.IsNullOrWhiteSpace(ejecutivoId))
        {
            query = query.Where(r => r.EjecutivoId == ejecutivoId);
        }
        if (clasificacionId.HasValue)
        {
            query = query.Where(r => r.ClasificacionSeguridadId == clasificacionId.Value);
        }
        if (topicIds?.Any() == true)
        {
            query = query.Where(r => r.RegistroTopics.Any(rt => topicIds.Contains(rt.TopicId)));
        }
        if (desde.HasValue)
        {
            query = query.Where(r => r.FechaCreacion >= desde.Value);
        }
        if (hasta.HasValue)
        {
            query = query.Where(r => r.FechaCreacion <= hasta.Value);
        }
        if (!string.IsNullOrWhiteSpace(textoLibre))
        {
            query = query.Where(r => r.Asunto.Contains(textoLibre) || (r.Comentario != null && r.Comentario.Contains(textoLibre)));
        }

        return await query.OrderByDescending(r => r.FechaCreacion).ToListAsync();
    }

    public async Task AsociarPdfAsync(Guid registroId, string pdfPath, Usuario usuarioActual)
    {
        var registro = await _context.Registros.FindAsync(registroId) ?? throw new InvalidOperationException("Registro no encontrado");
        registro.PdfPath = pdfPath;
        registro.FechaActualizacion = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(usuarioActual.Id, "ASOCIAR_PDF", nameof(Registro), registroId.ToString(), "PDF vinculado");
    }

    public async Task<Attachment> AgregarAdjuntoAsync(Guid registroId, Attachment adjunto, Stream contenido, Usuario usuarioActual)
    {
        var registro = await _context.Registros.FindAsync(registroId) ?? throw new InvalidOperationException("Registro no encontrado");
        adjunto.RegistroId = registroId;
        adjunto.StoredPath = await _fileStorage.SaveAsync(adjunto.FileName, contenido, "adjuntos");
        adjunto.FechaCreacion = DateTime.UtcNow;
        _context.Adjuntos.Add(adjunto);
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(usuarioActual.Id, "AGREGAR_ADJUNTO", nameof(Registro), registroId.ToString(), $"Adjunto {adjunto.FileName}");
        return adjunto;
    }

    public async Task RemoverAdjuntoAsync(Guid adjuntoId, Usuario usuarioActual)
    {
        var adjunto = await _context.Adjuntos.FindAsync(adjuntoId) ?? throw new InvalidOperationException("Adjunto no encontrado");
        await _fileStorage.DeleteAsync(adjunto.StoredPath);
        _context.Adjuntos.Remove(adjunto);
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(usuarioActual.Id, "ELIMINAR_ADJUNTO", nameof(Attachment), adjunto.Id.ToString(), adjunto.FileName);
    }

    public async Task<IReadOnlyList<string>> DescomprimirAdjuntoAsync(Guid adjuntoId, string destinoTemporal)
    {
        var adjunto = await _context.Adjuntos.FindAsync(adjuntoId) ?? throw new InvalidOperationException("Adjunto no encontrado");
        return await _fileStorage.ExtractCompressedAsync(adjunto.StoredPath, destinoTemporal);
    }

    private async Task ActualizarTopicsAsync(Registro registro, IEnumerable<int> topicIds)
    {
        _context.Entry(registro).Collection(r => r.RegistroTopics).Load();
        registro.RegistroTopics.Clear();
        foreach (var topicId in topicIds)
        {
            registro.RegistroTopics.Add(new RegistroTopic { RegistroId = registro.Id, TopicId = topicId });
        }
        await Task.CompletedTask;
    }
}
