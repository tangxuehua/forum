using Microsoft.AspNetCore.Http;

namespace Forum.Web.Services
{
    public interface IContextService
    {
        AccountIdentity GetCurrentAccount(HttpContext httpContext);
    }

    public class AccountIdentity
    {
        public string AccountId { get; private set; }
        public string AccountName { get; private set; }

        public AccountIdentity(string accountId, string accountName)
        {
            AccountId = accountId;
            AccountName = accountName;
        }
    }
}
