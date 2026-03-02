using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Prog4GodMAUI.Models;
using Prog4GodMAUI.Services;
using Prog4GodMAUI.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Prog4GodMAUI.ViewModels
{
    [QueryProperty(nameof(Product), "Product")]
    public partial class ProductDetailsViewModel : BaseViewModel
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;
        private readonly RecentlyViewedProductService _recentlyViewedProductService;

        public ProductDetailsViewModel(ProductService productService, CartService cartService, RecentlyViewedProductService recentlyViewedProductService)
        {
            _productService = productService;
            _cartService = cartService;
            _recentlyViewedProductService = recentlyViewedProductService;
        }


        [ObservableProperty]
        Product product;

        public ObservableCollection<Product> CrossSellProducts { get; private set; } = new ObservableCollection<Product>();

        [RelayCommand]
        public async Task Init()
        {
            await GetProductByIdAsync();
            await GetCrossSellProductsAsync();
        }

        private async Task GetProductByIdAsync() 
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;

                var product = await _productService.GetProductByIdAsync(Product.Id);
                Product = product;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get product: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Unable to get products", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task GetCrossSellProductsAsync()
        {
            if(IsBusy || Product == null)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var crossSellProducts = await _productService.GetProductsByCategoryAsync(Product.Category);
                CrossSellProducts.Clear();
                foreach (var crossSellProduct in crossSellProducts)
                {
                    if (crossSellProduct.Id != Product.Id)
                    {
                        CrossSellProducts.Add(crossSellProduct);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get cross-sell products: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Unable to get cross-sell products.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
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

            _recentlyViewedProductService.AddProduct(product);

            await Shell.Current.GoToAsync($"{nameof(ProductDetailsPage)}", true, navigationParameter);
            
            IsBusy = false;
        }

        [RelayCommand]
        private async Task ShareProduct(Product product)
        {
            if (product == null)
            {
                return;
            }
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = product.Image,
                Title = product.Title,
                Text = "HEY! Check out this product on GreenStore!",
            });
        }

        [RelayCommand]
        private async Task AddToCart(Product product)
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                if (product == null)
                {
                    return;
                }

                _cartService.AddProductToCart(product);

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                string infoText = "Product successfully added to cart!";
                ToastDuration duration = ToastDuration.Short;
                var toast = Toast.Make(infoText,duration);

                await toast.Show(cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to add product to cart: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "failed to add product to cart.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
