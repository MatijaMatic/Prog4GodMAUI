using Prog4GodMAUI.ViewModels;

namespace Prog4GodMAUI.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}