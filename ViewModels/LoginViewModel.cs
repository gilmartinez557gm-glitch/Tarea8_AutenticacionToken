using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tarea8_AutenticacionToken.Services;
using Tarea8_AutenticacionToken.Views;

namespace Tarea8_AutenticacionToken.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _mensaje = string.Empty;
    private bool _isBusy;

    public string Username
    {
        get => _username;
        set => CambiarPropiedad(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => CambiarPropiedad(ref _password, value);
    }

    public string Mensaje
    {
        get => _mensaje;
        set => CambiarPropiedad(ref _mensaje, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => CambiarPropiedad(ref _isBusy, value);
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;

        LoginCommand = new Command(async () =>
            await IniciarSesionAsync());
    }

    public async Task IniciarSesionAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password))
        {
            Mensaje = "Ingrese el usuario y la contraseña.";
            return;
        }

        try
        {
            IsBusy = true;
            Mensaje = string.Empty;

            var resultado = await _authService.IniciarSesionAsync(
                Username, Password);

            if (resultado == null ||
                string.IsNullOrWhiteSpace(resultado.AccessToken))
            {
                Mensaje = "La API no devolvió un token válido.";
                return;
            }

            await SecureStorage.Default.SetAsync(
                "access_token", resultado.AccessToken);

            Preferences.Default.Set(
                 "username", resultado.Username);

            Mensaje = "Inicio de sesión exitoso.";

            await Shell.Current.GoToAsync(
                nameof(ProfilePage));

            Mensaje = "Inicio de sesión exitoso.";
        }
        catch (HttpRequestException ex)
            when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            Mensaje = "Usuario o contraseña incorrectos.";
        }
        catch (HttpRequestException)
        {
            Mensaje = "No se pudo conectar con el servidor.";
        }
        catch (Exception)
        {
            Mensaje = "Ocurrió un error al iniciar sesión.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void CambiarPropiedad<T>(
        ref T campo,
        T valor,
        [CallerMemberName] string? nombre = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor))
            return;

        campo = valor;

        PropertyChanged?.Invoke(
            this, new PropertyChangedEventArgs(nombre));
    }
}
