using Prog4GodMAUI.ViewModels;

namespace Prog4GodMAUI.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}