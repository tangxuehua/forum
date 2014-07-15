using ECommon.Extensions;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Sections;
using Forum.Domain.Sections;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class SectionTest : TestBase
    {
        [Test]
        public void create_section_test()
        {
            var name = ObjectId.GenerateNewStringId();

            var result = _commandService.Execute(new CreateSectionCommand(name), CommandReturnType.EventHandled).WaitResult<CommandResult>(10000);

            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);

            var section = _memoryCache.Get<Section>(result.AggregateRootId);

            Assert.NotNull(section);
            Assert.AreEqual(result.AggregateRootId, section.Id);
            Assert.AreEqual(name, section.Name);
        }
        [Test]
        public void update_section_test()
        {
            var name = ObjectId.GenerateNewStringId();

            var result = _commandService.Execute(new CreateSectionCommand(name), CommandReturnType.EventHandled).WaitResult<CommandResult>(10000);

            var name2 = ObjectId.GenerateNewStringId();

            _commandService.Execute(new ChangeSectionNameCommand(result.AggregateRootId, name2), CommandReturnType.EventHandled).Wait();

            var section = _memoryCache.Get<Section>(result.AggregateRootId);

            Assert.NotNull(section);
            Assert.AreEqual(result.AggregateRootId, section.Id);
            Assert.AreEqual(name2, section.Name);
        }
    }
}
