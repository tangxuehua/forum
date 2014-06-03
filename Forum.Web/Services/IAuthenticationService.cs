namespace Forum.Web.Services
{
    public interface IAuthenticationService
    {
        void SignIn(string userId, string accountName, bool createPersistentCookie);
        void SignOut();
    }
}
