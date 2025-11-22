using GestorMensajesInstitucionales.Domain.Entities;

namespace GestorMensajesInstitucionales.Application.Interfaces;

public interface IUsuarioService
{
    Task<Usuario?> AutenticarAsync(string username, string password);
    Task<Usuario> CrearAsync(Usuario usuario, string password, Usuario actor);
    Task<Usuario> ActualizarAsync(Usuario usuario, Usuario actor);
    Task DesactivarAsync(int usuarioId, Usuario actor);
    Task<IEnumerable<Usuario>> ListarAsync();
}
