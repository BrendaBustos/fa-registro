using GestorMensajesInstitucionales.Domain.Entities;
using GestorMensajesInstitucionales.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GestorMensajesInstitucionales.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Registro> Registros => Set<Registro>();
    public DbSet<Attachment> Adjuntos => Set<Attachment>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<RegistroTopic> RegistroTopics => Set<RegistroTopic>();
    public DbSet<Promotor> Promotores => Set<Promotor>();
    public DbSet<Ejecutivo> Ejecutivos => Set<Ejecutivo>();
    public DbSet<ClasificacionSeguridad> Clasificaciones => Set<ClasificacionSeguridad>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Registro>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Asunto).IsRequired();
            entity.HasOne(e => e.Predecesor)
                .WithMany(e => e.Respuestas)
                .HasForeignKey(e => e.PredecesorId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Promotor)
                .WithMany(p => p.Registros)
                .HasForeignKey(e => e.PromotorId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Ejecutivo)
                .WithMany(p => p.Registros)
                .HasForeignKey(e => e.EjecutivoId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ClasificacionSeguridad)
                .WithMany(c => c.Registros)
                .HasForeignKey(e => e.ClasificacionSeguridadId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired();
            entity.Property(e => e.FileType).IsRequired();
            entity.Property(e => e.StoredPath).IsRequired();
            entity.HasOne(e => e.Registro)
                .WithMany(r => r.Adjuntos)
                .HasForeignKey(e => e.RegistroId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RegistroTopic>(entity =>
        {
            entity.HasKey(rt => new { rt.RegistroId, rt.TopicId });
            entity.HasOne(rt => rt.Registro)
                .WithMany(r => r.RegistroTopics)
                .HasForeignKey(rt => rt.RegistroId);
            entity.HasOne(rt => rt.Topic)
                .WithMany(t => t.RegistroTopics)
                .HasForeignKey(rt => rt.TopicId);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.Property(t => t.Nombre).IsRequired();
        });

        modelBuilder.Entity<Promotor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired();
        });

        modelBuilder.Entity<Ejecutivo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired();
        });

        modelBuilder.Entity<ClasificacionSeguridad>(entity =>
        {
            entity.Property(e => e.Nombre).IsRequired();
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.NombreCompleto).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.PasswordSalt).IsRequired();
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.Property(a => a.Accion).IsRequired();
            entity.Property(a => a.Entidad).IsRequired();
            entity.Property(a => a.EntidadId).IsRequired();
            entity.Property(a => a.Detalle).IsRequired();
            entity.HasOne(a => a.Usuario)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Topic>().HasData(
            new Topic { Id = 1, Nombre = "Antivirus", EsProyecto = false },
            new Topic { Id = 2, Nombre = "Firewall", EsProyecto = false },
            new Topic { Id = 3, Nombre = "Capacitación", EsProyecto = false },
            new Topic { Id = 4, Nombre = "Info de Personal", EsProyecto = false }
        );
    }
}
