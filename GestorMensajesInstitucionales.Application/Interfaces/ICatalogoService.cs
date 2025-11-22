using GestorMensajesInstitucionales.Domain.Entities;

namespace GestorMensajesInstitucionales.Application.Interfaces;

public interface ICatalogoService
{
    Task<IEnumerable<Promotor>> ListarPromotoresAsync();
    Task<IEnumerable<Ejecutivo>> ListarEjecutivosAsync();
    Task<IEnumerable<ClasificacionSeguridad>> ListarClasificacionesAsync();
    Task<IEnumerable<Topic>> ListarTopicsAsync();
    Task<Topic> CrearProyectoAsync(string nombreProyecto, Usuario actor);
}
