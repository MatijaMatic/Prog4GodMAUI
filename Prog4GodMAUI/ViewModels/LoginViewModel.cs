using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prog4GodMAUI.Models;
using Prog4GodMAUI.Services;
using System.Diagnostics;

namespace Prog4GodMAUI.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }


        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        bool isPasswordVisible;

        private LoginResponse loginResponse = new LoginResponse();

        [RelayCommand]
        public async Task Login()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                loginResponse = await _authService.LoginAsync(Username, Password);

                if (loginResponse != null)
                {
                    Debug.WriteLine($"Login successful. Token: {loginResponse.Token}");

                    //save token to secure storage
                    await SecureStorage.Default.SetAsync("token", loginResponse.Token);

                    await SecureStorage.Default.SetAsync("userId", loginResponse.UserId.ToString());

                    _authService.IsUserLoggedIn = true;

                    var navigationStack = Shell.Current.Navigation.NavigationStack;

                    if (navigationStack.Count >= 2)
                    {
                        await Shell.Current.Navigation.PopAsync();
                    }
                    else 
                    {
                        await Shell.Current.GoToAsync("//HomePage"); ;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        public void TogglePasswordVisibility()
        {
            this.IsPasswordVisible = !this.IsPasswordVisible;
        }
    }
}
