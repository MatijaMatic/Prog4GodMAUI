using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prog4GodMAUI.Services;
using Prog4GodMAUI.Models;
using Prog4GodMAUI.Views;

namespace Prog4GodMAUI.ViewModels
{
    public partial class RecentlyViewedPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        public RecentlyViewedProductService recentlyViewedProductService;

        public RecentlyViewedPageViewModel(RecentlyViewedProductService recentlyViewedProductService)
        {
            RecentlyViewedProductService = recentlyViewedProductService;
        }

        


        [RelayCommand]
        public async Task Init()
        {
            RecentlyViewedProductService.LoadProducts();

            await Task.CompletedTask;
        }

        [RelayCommand]
        private async Task ProductTapped(Product product)
        {
            IsBusy = true;

            if (product == null)
            {
                return;
            }

            var navigationParameter = new Dictionary<string, object>
            {
                {"Product", product },
            };

            await Shell.Current.GoToAsync($"{nameof(ProductDetailsPage)}", true, navigationParameter);

            IsBusy = false;
        }

        [RelayCommand]
        private async Task GoToHome()
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        [RelayCommand]
        private async Task DeleteAll()
        {
            var result = await Shell.Current.DisplayAlert("Delete all?", "Are you sure you want to delete all?", "Yes", "No");

            if (!result)
            {
                return;
            }

            RecentlyViewedProductService.RecentlyViewedProducts.Clear();
            RecentlyViewedProductService.SaveProduct();

            await Task.CompletedTask;
        }
    } 
}
