using System;
using System.IO;
using System.Windows;
using GestorMensajesInstitucionales.Infrastructure.Data;
using GestorMensajesInstitucionales.Infrastructure.Services;
using GestorMensajesInstitucionales.Infrastructure.Storage;
using GestorMensajesInstitucionales.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GestorMensajesInstitucionales.UI;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
        var login = _serviceProvider.GetRequiredService<Views.LoginWindow>();
        login.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite("Data Source=gestor.db");
        });

        services.AddSingleton<IFileStorage>(new FileSystemStorage(Path.Combine(AppContext.BaseDirectory, "storage")));
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ICatalogoService, CatalogoService>();
        services.AddScoped<IRegistroService, RegistroService>();

        services.AddScoped<Views.LoginWindow>();
        services.AddScoped<Views.MainWindow>();
    }
}
