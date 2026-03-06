using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prog4GodMAUI.Models;
using Prog4GodMAUI.Services;
using Prog4GodMAUI.Views;
using System.Diagnostics;

namespace Prog4GodMAUI.ViewModels
{
    public partial class ProfilePageViewModel : BaseViewModel
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public ProfilePageViewModel(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        


        [ObservableProperty]
        public bool isUserLoggedIn;
        
        [ObservableProperty]
        User user;

        [RelayCommand]
        public async Task Init()
        {
            await GetUserByIdAsync();
            isUserLoggedIn = _authService.IsUserLoggedIn;
        }

        private async Task GetUserByIdAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var userId = await SecureStorage.Default.GetAsync("userId");

                if (userId == null) 
                {
                    Debug.WriteLine("User id not found in secure storage.");
                    return;
                }
                else
                {
                    var user = await _userService.GetUserAsync(int.Parse(userId));

                    if (user != null)
                    {
                        User = user;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get user: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to retrieve user.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            var result = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");

            if (result)
            {
                _authService.IsUserLoggedIn = false;

                await Shell.Current.GoToAsync("//HomePage");
            }
        }

        [RelayCommand]
        private async Task Login()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        [RelayCommand]
        private static async Task OpenFakeStoreApi()
        {
            await Browser.OpenAsync("https://fakestoreapi.com/");
        }

        [RelayCommand]
        private static async Task LinkedIn()
        {
            await Browser.OpenAsync("https://github.com/MatijaMatic");
        }
    
        [RelayCommand]
        private async Task DeleteAccount()
        {
            var result = await Shell.Current.DisplayAlert("Delete account?", "are you sure you want to delete your account", "Yes", "No");

            if (result)
            {
                var userId = await SecureStorage.Default.GetAsync("userId");

                if (userId == null)
                {
                    Debug.WriteLine("User id not found in secure storage!");
                    return;
                }
                else
                {
                    var isDeleted = await _userService.DeleteUserAccountAsync(int.Parse(userId));

                    if (isDeleted)
                    {
                        await Shell.Current.DisplayAlert("Info", "Account deleted successfully.", "OK");

                        await Shell.Current.GoToAsync("//HomePage");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Failed to delete account.", "OK");
                    }
                }
            }
        }
        
    }
}
