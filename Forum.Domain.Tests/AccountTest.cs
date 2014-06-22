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

            var result = _commandService.StartProcess(new StartRegistrationCommand(name, password)).WaitResult<ProcessResult>(10000);

            Assert.AreEqual(ProcessStatus.Success, result.Status);
            Assert.IsTrue(result.Items.ContainsKey("AccountId"));
            Assert.IsNotNull(result.Items["AccountId"]);

            var account = _memoryCache.Get<Account>(result.Items["AccountId"]);
            Assert.NotNull(account);
            Assert.AreEqual(name, account.AccountInfo.Name);
            Assert.AreEqual(password, account.AccountInfo.Password);

            var accountInfo = ObjectContainer.Resolve<IAccountQueryService>().Find(name);
            Assert.IsNotNull(accountInfo);
            Assert.AreEqual(result.Items["AccountId"], accountInfo.Id);
            Assert.AreEqual(name, accountInfo.Name);
            Assert.AreEqual(password, accountInfo.Password);
        }

        [Test]
        public void create_duplicate_account_test()
        {
            var name = ObjectId.GenerateNewStringId();
            var password = ObjectId.GenerateNewStringId();

            _commandService.StartProcess(new StartRegistrationCommand(name, password)).WaitResult<ProcessResult>(10000);
            var result = _commandService.StartProcess(new StartRegistrationCommand(name, password)).WaitResult<ProcessResult>(10000);

            Assert.AreEqual(ProcessStatus.Failed, result.Status);
            Assert.AreEqual(Forum.Infrastructure.ErrorCodes.RegistrationDuplicateAccount, result.ErrorCode);
        }
    }
}
