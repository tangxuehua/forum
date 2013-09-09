using System;
using System.Collections.Generic;
using ENode.Infrastructure;
using ENode.Infrastructure.Serializing;
using Forum.Domain.Accounts.Model;
using Forum.Domain.Accounts.Repositories;

namespace Forum.Domain.Test.Stubs
{
    [Component(LifeStyle.Singleton)]
    public class AccountRegistrationInfoRepository : IAccountRegistrationInfoRepository
    {
        private readonly IBinarySerializer _binarySerializer;
        private readonly IDictionary<string, byte[]> _registrationInfoDict = new Dictionary<string, byte[]>();

        public AccountRegistrationInfoRepository(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public void Add(AccountRegistrationInfo info)
        {
            try
            {
                _registrationInfoDict.Add(info.AccountName, _binarySerializer.Serialize(info));
            }
            catch (ArgumentException ex)
            {
                throw new DuplicateAccountNameException(info.AccountName, ex);
            }
        }
        public void Update(AccountRegistrationInfo info)
        {
            _registrationInfoDict[info.AccountName] = _binarySerializer.Serialize(info);
        }
        public AccountRegistrationInfo Get(string accountName)
        {
            return _binarySerializer.Deserialize<AccountRegistrationInfo>(_registrationInfoDict[accountName]);
        }
    }
}
