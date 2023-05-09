using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient;

namespace App.Services
{
    interface IAuthService
    {
        Task<LoginResult> LoginAsync();
        Task<BrowserResult> LogoutAsync();
        bool IsAuthenticated { get; }
        string CurrentUser { get; }
    }
}
