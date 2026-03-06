using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prog4GodMAUI.Models;
using Prog4GodMAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Prog4GodMAUI.ViewModels
{
    public partial class CartViewModel : BaseViewModel
    {
        private readonly CartService _cartService;
        private readonly AuthService _authService;

        bool isFirstRun;

        public CartViewModel(CartService cartService, AuthService authService)
        {
            _cartService = cartService;
            _authService = authService;
            isFirstRun = true;
        }

        [ObservableProperty]
        public bool isUserLoggedIn;

        [ObservableProperty]
        private bool isBusyWithCartModification;

        public ObservableCollection<CartItemDetail> CartItems { get; private set; } = new ObservableCollection<CartItemDetail>();

        [RelayCommand]
        public async Task Init()
        {
            if (isFirstRun)
            {
                await GetCartByUserIdAsync();
                isFirstRun = false;
            }
            else
            {
                this.SyncCartItems();
            }
            IsUserLoggedIn = _authService.IsUserLoggedIn;
        }

        private async Task GetCartByUserIdAsync()
        {
            if (_authService.IsUserLoggedIn)
            {
                if (IsBusy)
                {
                    return;
                }

                int userId;
                var userIdStr = await SecureStorage.GetAsync("userId");
                if (!int.TryParse(userIdStr, out userId))
                {
                    Debug.WriteLine("Failed to get or parse userId from SecureStorage.");
                    await Shell.Current.DisplayAlert("Error", "Failed to retrieve user.", "OK");
                    return;
                }

                try
                {
                    IsBusy = true;

                    await _cartService.RefreshCartItemsByUserIdAsync(userId);
                    CartItems.Clear();
                    foreach (var cartItem in _cartService.GetCartItems())
                    {
                        CartItems.Add(cartItem);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unable to get cart: {ex.Message}");
                    await Shell.Current.DisplayAlert("Error", "Failed to retrieve cart.", "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        [RelayCommand]
        public async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync("LoginPage");
        }

        [RelayCommand]
        public async Task DeleteCart()
        {
            if (IsBusy || IsBusyWithCartModification)
            {
                return;
            }

            try
            {
                var userResponse = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to delete the cart?", "Yes", "No");
                if (!userResponse)
                {
                    Debug.WriteLine("Cart deletion cancelled by the user.");
                    return;
                }

                if (CartItems.Count > 0)
                {
                    IsBusyWithCartModification = true;

                    var response = await _cartService.DeleteCartAsync();

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Cart deleted.");
                        CartItems.Clear();

                        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                        string infoText = "Cart deleted successfully.";
                        ToastDuration duration = ToastDuration.Short;
                        var toast = Toast.Make(infoText, duration);

                        await toast.Show(cancellationTokenSource.Token);
                    }
                    else
                    {
                        Debug.WriteLine("Failed to delete cart.");
                        await Shell.Current.DisplayAlert("Error", "Failed to delete cart.", "OK");
                    }
                }
                else
                {
                    Debug.WriteLine("No cart found.");
                    await Shell.Current.DisplayAlert("Error", "No cart found.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to delete cart: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to delete cart.", "OK");
            }
            finally
            {
                IsBusyWithCartModification = false;
            }
        }

        [RelayCommand]
        public void IncreaseProductQuantity(Product product)
        {
            try
            {
                _cartService.IncreaseProductQuantity(product.Id);
                SyncCartItems();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to increase product quantity: {ex.Message}");
                Shell.Current.DisplayAlert("Error", "Failed to increase product quantity.", "OK");
            }
        }

        [RelayCommand]
        public void DecreaseProductQuantity(Product product)
        {
            try
            {
                _cartService.DecreaseProductQuantity(product.Id);
                SyncCartItems();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to decrease product quantity: {ex.Message}");
                Shell.Current.DisplayAlert("Error", "Failed to decrease product quantity.", "OK");
            }
        }

        private void SyncCartItems()
        {
            var updatedCartItems = _cartService.GetCartItems();
            foreach (var item in CartItems.ToList())
            {
                if (!updatedCartItems.Any(ci => ci.Product.Id == item.Product.Id))
                {
                    CartItems.Remove(item);
                }
            }

            foreach (var updatedItem in updatedCartItems)
            {
                var existingItem = CartItems.FirstOrDefault(ci => ci.Product.Id == updatedItem.Product.Id);
                if (existingItem == null)
                {
                    CartItems.Add(updatedItem);
                }
                else
                {
                    existingItem.Quantity = updatedItem.Quantity;
                }
            }
        }
    }
}
