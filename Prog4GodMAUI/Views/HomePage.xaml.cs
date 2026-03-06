using Prog4GodMAUI.ViewModels;

namespace Prog4GodMAUI.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
