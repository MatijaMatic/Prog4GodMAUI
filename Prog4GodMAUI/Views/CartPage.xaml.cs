namespace Prog4GodMAUI.Views;
using Prog4GodMAUI.ViewModels;

public partial class CartPage : ContentPage
{
	public CartPage(CartViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}