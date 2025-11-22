using System;
using System.Windows;
using GestorMensajesInstitucionales.Domain.Entities;
using GestorMensajesInstitucionales.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GestorMensajesInstitucionales.UI.Views;

public partial class MainWindow : Window
{
    private readonly IRegistroService _registroService;
    private readonly Usuario _usuarioActual;

    public MainWindow(Usuario usuarioActual)
    {
        InitializeComponent();
        _usuarioActual = usuarioActual;
        _registroService = ((App)Application.Current).Services().GetRequiredService<IRegistroService>();
        CargarRegistros();
    }

    private async void CargarRegistros()
    {
        var registros = await _registroService.BuscarAsync(null, null, null, null, null, null, null, null, null);
        RegistrosGrid.ItemsSource = registros;
    }

    private void OnFiltrarClick(object sender, RoutedEventArgs e)
    {
        // Placeholder para lógica de filtro; en una implementación completa se mapearían valores de UI a parámetros del servicio.
        CargarRegistros();
    }

    private void OnNuevoClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Abrir ventana de alta/edición de registro (no implementada en este esqueleto).", "Nuevo registro");
    }
}

public static class AppExtensions
{
    public static IServiceProvider Services(this App app)
    {
        return (IServiceProvider)app.GetType()
            .GetField("_serviceProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .GetValue(app)!;
    }
}
