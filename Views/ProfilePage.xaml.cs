using Tarea8_AutenticacionToken.ViewModels;

namespace Tarea8_AutenticacionToken.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.Usuario == null)
            await _viewModel.CargarPerfilAsync();
    }
}
