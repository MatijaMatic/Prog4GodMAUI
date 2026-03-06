using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prog4GodMAUI.Models;
using Prog4GodMAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Prog4GodMAUI.ViewModels
{
    [QueryProperty(nameof(Category), "Category")]
    public partial class CategoryPageViewModel : BaseViewModel
    {
        private readonly ProductService _productService;
        private readonly RecentlyViewedProductService _recentlyViewedProductService;

        public CategoryPageViewModel(ProductService productService, RecentlyViewedProductService recentlyViewedProductService)
        {
            _productService = productService;
            _recentlyViewedProductService = recentlyViewedProductService;
        }

        [ObservableProperty]
        Category category;

        [ObservableProperty]
        string sortOrder = "asc";

        [ObservableProperty]
        bool isBusyWithSorting = false;

        public ObservableCollection<Product> Products { get; private set; } = new ObservableCollection<Product>();

        [RelayCommand]
        public async Task Init()
        {
            await GetProductsByCategoryAsync();
        }

        private async Task GetProductsByCategoryAsync(string sortOrder = "asc")
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = !isBusyWithSorting;
                var products = await _productService.GetProductsByCategoryAsync(Category.Name, sortOrder);
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get products: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
