using ECommon.Components;
using ECommon.Extensions;
using ECommon.Logging;
using ECommon.Utilities;
using ENode.Commanding;
using ENode.Infrastructure;
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

            var result = ExecuteCommand(new CreateSectionCommand(name));

            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);

            var section = _sectionQueryService.FindDynamic(result.AggregateRootId, "simple");

            Assert.NotNull(section);
            Assert.AreEqual(result.AggregateRootId, section.id);
            Assert.AreEqual(name, section.name);
        }
        [Test]
        public void update_section_test()
        {
            var name = ObjectId.GenerateNewStringId();
            var result = ExecuteCommand(new CreateSectionCommand(name));
            var name2 = ObjectId.GenerateNewStringId();
            var result2 = ExecuteCommand(new ChangeSectionNameCommand(result.AggregateRootId, name2));

            Assert.AreEqual(CommandStatus.Success, result2.Status);

            var section = _sectionQueryService.FindDynamic(result.AggregateRootId, "simple");

            Assert.NotNull(section);
            Assert.AreEqual(result.AggregateRootId, section.id);
            Assert.AreEqual(name2, section.name);
        }
    }
}
