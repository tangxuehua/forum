using System;
using Forum.Commands.Section;
using Forum.Domain.Sections;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class SectionTest : TestBase
    {
        public static Guid SectionId;

        [Test]
        public void CreateSectionTest()
        {
            var sectionName = RandomString();
            ResetWaiters();
            Section section = null;

            CommandService.Send(new CreateSection { SectionName = sectionName }, (result) =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                section = MemoryCache.Get<Section>(SectionId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(section);
            Assert.AreEqual(sectionName, section.Name);
        }
        [Test]
        public void ChangeSectionNameTest()
        {
            CreateSectionTest();
            Section section = null;

            ResetWaiters();
            var newSectionName = RandomString();
            CommandService.Send(new ChangeSectionName { SectionId = SectionId, SectionName = newSectionName }, (result) =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                section = MemoryCache.Get<Section>(SectionId.ToString());
                TestThreadWaiter.Set();
            });
            TestThreadWaiter.WaitOne(500);
            Assert.AreEqual(newSectionName, section.Name);
        }
    }
}
