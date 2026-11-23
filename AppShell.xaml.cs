using Tarea8_AutenticacionToken.Views;

namespace Tarea8_AutenticacionToken;

public partial class AppShell : Shell
{
    private bool _sesionVerificada;

    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(
            nameof(ProfilePage),
            typeof(ProfilePage));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_sesionVerificada)
            return;

        _sesionVerificada = true;

        try
        {
            string? token = await SecureStorage.Default.GetAsync(
                "access_token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                await GoToAsync(nameof(ProfilePage));
            }
        }
        catch
        {
            SecureStorage.Default.Remove("access_token");
        }
    }
}
