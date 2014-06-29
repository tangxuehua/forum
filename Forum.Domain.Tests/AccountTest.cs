using ECommon.Components;
using ECommon.Extensions;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Accounts;
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

            var result = _commandService.StartProcess(new RegisterNewAccountCommand(name, password)).WaitResult<ProcessResult>(10000);

            Assert.AreEqual(ProcessStatus.Success, result.Status);

            var account = ObjectContainer.Resolve<IAccountQueryService>().Find(name);
            Assert.IsNotNull(account);
            Assert.AreEqual(name, account.Name);
            Assert.AreEqual(password, account.Password);
        }

        [Test]
        public void create_duplicate_account_test()
        {
            var name = ObjectId.GenerateNewStringId();
            var password = ObjectId.GenerateNewStringId();

            _commandService.StartProcess(new RegisterNewAccountCommand(name, password)).WaitResult<ProcessResult>(10000);
            var result = _commandService.StartProcess(new RegisterNewAccountCommand(name, password)).WaitResult<ProcessResult>(10000);

            Assert.AreEqual(ProcessStatus.Failed, result.Status);
            Assert.AreEqual(Forum.Infrastructure.ErrorCodes.DuplicateAccount, result.ErrorCode);
        }
    }
}
