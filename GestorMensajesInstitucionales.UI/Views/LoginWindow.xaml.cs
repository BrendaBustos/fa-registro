using System.Windows;
using GestorMensajesInstitucionales.Application.Interfaces;

namespace GestorMensajesInstitucionales.UI.Views;

public partial class LoginWindow : Window
{
    private readonly IUsuarioService _usuarioService;

    public LoginWindow(IUsuarioService usuarioService)
    {
        InitializeComponent();
        _usuarioService = usuarioService;
    }

    private async void OnLoginClick(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        var password = PasswordBox.Password;
        var usuario = await _usuarioService.AutenticarAsync(username, password);
        if (usuario is null)
        {
            MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var main = new MainWindow(usuario);
        main.Show();
        Close();
    }
}
