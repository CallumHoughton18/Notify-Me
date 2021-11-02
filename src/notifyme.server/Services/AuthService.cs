using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using notifyme.shared.Models;
using notifyme.shared.Service_Interfaces;

namespace notifyme.server.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }
        public async Task<User> GetCurrentUserAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.Identity != null ? new User(user.Identity.Name) : null;
        }
    }
}