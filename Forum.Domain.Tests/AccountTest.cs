using ECommon.IoC;
using ECommon.Utilities;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;
using Forum.QueryServices;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class AccountTest : TestBase
    {
        [Test]
        public void create_single_account_test()
        {
            var id = ObjectId.GenerateNewStringId();
            var name = ObjectId.GenerateNewStringId();
            var password = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateAccountCommand(id, name, password)).Wait();

            var account = _memoryCache.Get<Account>(id);
            Assert.NotNull(account);
            Assert.AreEqual(id, account.Id);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(password, account.Password);

            account = ObjectContainer.Resolve<IAccountService>().GetAccount(name);
            Assert.IsNotNull(account);
            Assert.AreEqual(id, account.Id);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(password, account.Password);
        }
    }
}
