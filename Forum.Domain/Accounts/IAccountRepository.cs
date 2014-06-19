namespace Forum.Domain.Accounts
{
    public interface IAccountRepository
    {
        void Add(Registration account);
        Registration Find(string name);
        string GetNextAccountId();
    }
}
