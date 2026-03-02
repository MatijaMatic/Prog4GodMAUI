using Prog4GodMAUI.ViewModels;

namespace Prog4GodMAUI.Views;

public partial class CategoryPage : ContentPage
{
	public CategoryPage(CategoryPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}