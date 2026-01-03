using System.Security.Claims;

namespace CORE.APP.Services.Authentication
{
    public abstract class AuthServiceBase : ServiceBase
    {
        protected List<Claim> GetClaims(int userId, string userName, string[] roleNames)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", userId.ToString()),
                new Claim(ClaimTypes.Name, userName)
            };
            foreach (var roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }
            return claims;
        }
    }
}
