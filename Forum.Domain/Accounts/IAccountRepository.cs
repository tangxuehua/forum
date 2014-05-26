namespace Forum.Domain.Accounts
{
    public interface IAccountRepository
    {
        void Add(AccountInfo account);
        AccountInfo Find(string name);
        string GetNextAccountId();
    }
}
