using Prog4GodMAUI.ViewModels;

namespace Prog4GodMAUI.Views;

public partial class ProductDetailsPage : ContentPage
{
	public ProductDetailsPage(ProductDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}