using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Text.Json;

namespace Prog4GodMAUI.Services
{
    public class BaseService
    {
        protected readonly HttpClient _httpClient;

        public BaseService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://fakestoreapi.com/"),
            };
        }

        protected async Task<T> GetAsync<T>(string endpoint)
        {
            if (!IsInternetAvailable())
            {
                return default;
            }

            try
            {
                var response = await _httpClient.GetAsync(endpoint);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<T>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get data: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Unable to get data. Please try again later.", "OK");
                return default;
            }
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            if(!IsInternetAvailable())
            {
                return null;
            }

            try
            {
                return await _httpClient.DeleteAsync(endpoint);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to delete data: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Unable to delete data", "OK");
                return null;
            }
        }

        private bool IsInternetAvailable()
        {
            NetworkAccess accessType = Connectivity.NetworkAccess;

            if (accessType != NetworkAccess.Internet)
            {
                if (Shell.Current != null)
                {
                    if (accessType == NetworkAccess.ConstrainedInternet)
                    {
                        Shell.Current.DisplayAlert("Error!", "Internet access is limited.", "OK");
                    }
                    else 
                    {
                        Shell.Current.DisplayAlert("Error!", "No internet access.", "OK");
                    }
                }
                return false;
            }
            return true;
        }
    }
}
