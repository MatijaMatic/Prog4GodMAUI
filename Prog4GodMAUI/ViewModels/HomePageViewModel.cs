using Prog4GodMAUI.Services;
using Prog4GodMAUI.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using Prog4GodMAUI.Views;

namespace Prog4GodMAUI.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly RecentlyViewedProductService _recentlyViewedProductsService;

        /// <summary>
        /// Gets the products.
        /// </summary>
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();

        private bool isFirstRun;


        public HomePageViewModel(ProductService productService, CategoryService categoryService, RecentlyViewedProductService recentlyViewedProductsService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _recentlyViewedProductsService = recentlyViewedProductsService;
            isFirstRun = true;
        }



        [RelayCommand]
        public async Task Init()
        {
            if (isFirstRun)
            {
                await GetProductsAsync();
                await GetCategoriesAsync();
                _recentlyViewedProductsService.LoadProducts();
                isFirstRun = false;
            }
        }

        private async Task GetCategoriesAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private async Task GetProductsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                var products = await _productService.GetProductsAsync();
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

        [RelayCommand]
        private async Task ProductTapped(Product product)
        {
            if (product == null)
            {
                return;
            }

            var navigationParameter = new Dictionary<string, object>
            {
                { "Product", product },
            };

            _recentlyViewedProductsService.AddProduct(product);

            await Shell.Current.GoToAsync($"{nameof(ProductDetailsPage)}", true, navigationParameter);
        }

        
        [RelayCommand]
        private async Task CategoryTapped(Category category)
        {
            if (category == null)
            {
                return;
            }

            var navigationParameter = new Dictionary<string, object>
            {
                { "Category", category },
            };

            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}", true, navigationParameter);
        }
    }
}
