using Prog4GodMAUI.Models;

namespace Prog4GodMAUI.Services
{
    public class UserService :  BaseService
    {
        private readonly AuthService _authService;

        public UserService(AuthService authService)
        {
              _authService = authService;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await GetAsync<User>($"users/{userId}");
        }

        public async Task<bool> DeleteUserAccountAsync(int userId)
        {
            var response = await DeleteAsync($"users/{userId}");
            
            if (response !=null&&response.IsSuccessStatusCode)
            {
                _authService.IsUserLoggedIn = false;
                return true;
            }

            return false;
        }
    }
}
