using System;
using System.Linq;
using Prog4GodMAUI.Models;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Diagnostics;

namespace Prog4GodMAUI.Services
{
    public class AuthService : BaseService
    {

        public AuthService() { }

        public bool IsUserLoggedIn
        {
            get
            {
                var token = SecureStorage.GetAsync("token").Result;
                return !string.IsNullOrEmpty(token);
            }
            set
            {
                if (!value)
                {
                    SecureStorage.Remove("token");
                    SecureStorage.Remove("userId");
                }
            }

        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("auth/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Login failed. Status code: {response.StatusCode}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);

                // fetch all users (best-effort) and map user id
                try
                {
                    var usersResponse = await _httpClient.GetAsync("users");
                    if (usersResponse.IsSuccessStatusCode)
                    {
                        var usersResponseContent = await usersResponse.Content.ReadAsStringAsync();
                        var users = JsonSerializer.Deserialize<List<User>>(usersResponseContent);
                        var user = users?.FirstOrDefault(u => u.Username == username);
                        if (user != null && loginResponse != null)
                        {
                            loginResponse.UserId = user.Id;
                        }
                    }
                }
                catch (Exception exUsers)
                {
                    Debug.WriteLine($"Unable to fetch users: {exUsers.Message}");
                }

                return loginResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }
    }
}
