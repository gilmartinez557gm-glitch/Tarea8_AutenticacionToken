using Tarea8_AutenticacionToken.ViewModels;

namespace Tarea8_AutenticacionToken.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}
