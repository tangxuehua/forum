using ECommon.Components;
using ECommon.Extensions;
using ECommon.Utilities;
using ENode.Commanding;
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
            var name = ObjectId.GenerateNewStringId();
            var password = ObjectId.GenerateNewStringId();

            var result = _commandService.Execute(new CreateAccountCommand(name, password)).WaitResult<CommandResult>(3000);

            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);

            var account = _memoryCache.Get<Account>(result.AggregateRootId);
            Assert.NotNull(account);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(password, account.Password);

            var accountInfo = ObjectContainer.Resolve<IAccountQueryService>().Find(name);
            Assert.IsNotNull(accountInfo);
            Assert.AreEqual(result.AggregateRootId, accountInfo.Id);
            Assert.AreEqual(name, accountInfo.Name);
            Assert.AreEqual(password, accountInfo.Password);
        }

        [Test]
        public void create_duplicate_account_test()
        {
            var name = ObjectId.GenerateNewStringId();
            var password = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateAccountCommand(name, password)).Wait();
            var result = _commandService.Execute(new CreateAccountCommand(name, password)).WaitResult<CommandResult>(3000);

            Assert.AreEqual(CommandStatus.Failed, result.Status);
            Assert.AreEqual(typeof(DuplicateAccountNameException).Name, result.ExceptionTypeName);
        }
    }
}
