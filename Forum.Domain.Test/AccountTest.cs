using System;
using System.Threading;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Application.Commands;
using Forum.Domain.Events;
using Forum.Domain.Model;
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

            _commandService.Send(new CreateAccount { Name = name, Password = password }, (result) =>
            {
                Assert.IsFalse(result.HasError);
                EventHandlerWaiter.WaitOne();
                account = _memoryCache.Get<Account>(AccountId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(account);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(password, account.Password);
        }
    }
}
