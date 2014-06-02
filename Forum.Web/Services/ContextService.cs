using System.Web;
using System.Web.Security;

namespace Forum.Web.Services
{
    public class ContextService
    {
        public static AccountIdentity CurrentAccount
        {
            get
            {
                var user = HttpContext.Current.User;
                if (user != null &&
                    user.Identity != null &&
                    user.Identity.IsAuthenticated &&
                    user.Identity is FormsIdentity)
                {
                    var accountId = ((FormsIdentity)user.Identity).Ticket.UserData;
                    var accountName = user.Identity.Name;
                    return new AccountIdentity(accountId, accountName);
                }
                return null;
            }
        }
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