using System;
using Forum.Domain.Model;

namespace Forum.Domain {
    public interface IAccountRepository {
        void AddAccount(Guid accountId, string name);
        void ConfirmAccount(Guid accountId);
        Account GetAccount(string name);
    }
}
