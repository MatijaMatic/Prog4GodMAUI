using Microsoft.Extensions.DependencyInjection;
using Prog4GodMAUI.ViewModels;

namespace Prog4GodMAUI.Views;

public partial class RecentlyViewedPage : ContentPage
{
    public RecentlyViewedPage(RecentlyViewedPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
