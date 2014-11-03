using System.Web;
using System.Web.Security;
using ECommon.Components;

namespace Forum.Web.Services
{
    [Component]
    public class ContextService : IContextService
    {
        public AccountIdentity CurrentAccount
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
}