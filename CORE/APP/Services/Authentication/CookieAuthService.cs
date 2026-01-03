using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CORE.APP.Services.Authentication
{
    public class CookieAuthService : AuthServiceBase, ICookieAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieAuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SignIn(int userId, string userName, string[] userRoleNames, DateTime? expiration = default, bool isPersistent = true)
        {
            var claims = GetClaims(userId, userName, userRoleNames);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            var principal = new ClaimsPrincipal(identity);
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = expiration.HasValue ? DateTime.SpecifyKind(expiration.Value, DateTimeKind.Utc) : null
            };
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);
        }

        public async Task SignOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}
