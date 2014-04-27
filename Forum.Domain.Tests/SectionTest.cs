using ECommon.Utilities;
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
            var id = ObjectId.GenerateNewStringId();
            var name = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateSectionCommand(id, name)).Wait();

            var section = _memoryCache.Get<Section>(id);

            Assert.NotNull(section);
            Assert.AreEqual(id, section.Id);
            Assert.AreEqual(name, section.Name);
        }
        [Test]
        public void update_section_test()
        {
            var id = ObjectId.GenerateNewStringId();
            var name = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateSectionCommand(id, name)).Wait();

            var name2 = ObjectId.GenerateNewStringId();

            _commandService.Execute(new UpdateSectionCommand(id, name2)).Wait();

            var section = _memoryCache.Get<Section>(id);

            Assert.NotNull(section);
            Assert.AreEqual(id, section.Id);
            Assert.AreEqual(name2, section.Name);
        }
    }
}
