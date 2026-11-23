using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tarea8_AutenticacionToken.Models;
using Tarea8_AutenticacionToken.Services;

namespace Tarea8_AutenticacionToken.ViewModels;

public class ProfileViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;

    private Usuario? _usuario;
    private bool _isBusy;
    private string _mensaje = string.Empty;

    public Usuario? Usuario
    {
        get => _usuario;
        set => CambiarPropiedad(ref _usuario, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => CambiarPropiedad(ref _isBusy, value);
    }

    public string Mensaje
    {
        get => _mensaje;
        set => CambiarPropiedad(ref _mensaje, value);
    }

    public ICommand CerrarSesionCommand { get; }

    public ProfileViewModel(AuthService authService)
    {
        _authService = authService;

        CerrarSesionCommand = new Command(async () =>
            await CerrarSesionAsync());
    }

    public async Task CargarPerfilAsync()
    {
        try
        {
            IsBusy = true;
            Mensaje = string.Empty;

            string? token = await SecureStorage.Default.GetAsync(
                "access_token");

            if (string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            Usuario = await _authService.ObtenerPerfilAsync(token);

            if (Usuario == null)
                Mensaje = "No se pudo obtener el perfil.";
        }
        catch (HttpRequestException)
        {
            Mensaje = "No se pudo conectar con el servidor.";
        }
        catch (Exception)
        {
            Mensaje = "Ocurrió un error al cargar el perfil.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CerrarSesionAsync()
    {
        SecureStorage.Default.Remove("access_token");
        Preferences.Default.Remove("username");

        Usuario = null;

        await Shell.Current.GoToAsync("//LoginPage");
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
