using System;
using ENode.Infrastructure;
using Forum.Application.Commands.Account;
using Forum.Domain.Accounts.Model;
using Forum.Domain.Accounts.Services;
using NUnit.Framework;

namespace Forum.Domain.Test
{
    [TestFixture]
    public class AccountTest : TestBase
    {
        public static Guid AccountId;

        [Test]
        public void CreateAccountTest()
        {
            var name = RandomString();
            var password = RandomString();
            ResetWaiters();
            Account account = null;

            CommandService.Send(new CreateAccount { Name = name, Password = password }, result =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                account = MemoryCache.Get<Account>(AccountId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(account);
            Assert.AreEqual(AccountId, account.Id);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(password, account.Password);

            account = ObjectContainer.Resolve<IAccountService>().GetAccount(name);
            Assert.IsNotNull(account);
            Assert.AreEqual(AccountId, account.Id);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(password, account.Password);
        }
    }
}
