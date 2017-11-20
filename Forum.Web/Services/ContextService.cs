using System.Security.Claims;
using ECommon.Components;
using Microsoft.AspNetCore.Http;

namespace Forum.Web.Services
{
    [Component]
    public class ContextService : IContextService
    {
        public AccountIdentity GetCurrentAccount(HttpContext httpContext)
        {
            if (httpContext.User.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
            {
                var accountId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var accountName = identity.FindFirst(ClaimTypes.Name).Value;
                return new AccountIdentity(accountId, accountName);
            }
            return null;
        }
    }
}