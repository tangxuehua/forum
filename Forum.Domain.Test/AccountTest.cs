using System;
using Forum.Application.Commands;
using Forum.Domain.Model.Account;
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

            CommandService.Send(new CreateAccount { Name = name, Password = password }, (result) =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                account = MemoryCache.Get<Account>(AccountId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(account);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(password, account.Password);
        }
    }
}
