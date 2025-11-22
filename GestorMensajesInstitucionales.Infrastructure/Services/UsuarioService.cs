using System.Security.Cryptography;
using System.Text;
using GestorMensajesInstitucionales.Application.Interfaces;
using GestorMensajesInstitucionales.Domain.Entities;
using GestorMensajesInstitucionales.Domain.Enums;
using GestorMensajesInstitucionales.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestorMensajesInstitucionales.Infrastructure.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;
    private readonly IAuditService _auditService;

    public UsuarioService(AppDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<Usuario?> AutenticarAsync(string username, string password)
    {
        var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Username == username && u.Activo);
        if (usuario is null)
        {
            return null;
        }

        var hash = GenerarHash(password, usuario.PasswordSalt);
        if (!hash.SequenceEqual(usuario.PasswordHash))
        {
            return null;
        }

        usuario.UltimoAcceso = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(usuario.Id, "LOGIN", nameof(Usuario), usuario.Id.ToString(), "Ingreso al sistema");
        return usuario;
    }

    public async Task<Usuario> CrearAsync(Usuario usuario, string password, Usuario actor)
    {
        usuario.PasswordSalt = RandomNumberGenerator.GetBytes(16);
        usuario.PasswordHash = GenerarHash(password, usuario.PasswordSalt);
        usuario.FechaCreacion = DateTime.UtcNow;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(actor.Id, "CREAR", nameof(Usuario), usuario.Id.ToString(), $"Creación de usuario {usuario.Username}");
        return usuario;
    }

    public async Task<Usuario> ActualizarAsync(Usuario usuario, Usuario actor)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(actor.Id, "ACTUALIZAR", nameof(Usuario), usuario.Id.ToString(), "Actualización de usuario");
        return usuario;
    }

    public async Task DesactivarAsync(int usuarioId, Usuario actor)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId) ?? throw new InvalidOperationException("Usuario no encontrado");
        usuario.Activo = false;
        await _context.SaveChangesAsync();
        await _auditService.RegistrarAsync(actor.Id, "DESACTIVAR", nameof(Usuario), usuario.Id.ToString(), "Desactivación de usuario");
    }

    public async Task<IEnumerable<Usuario>> ListarAsync()
    {
        return await _context.Usuarios.AsNoTracking().ToListAsync();
    }

    private static byte[] GenerarHash(string password, byte[] salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, 100000, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(32);
    }
}
