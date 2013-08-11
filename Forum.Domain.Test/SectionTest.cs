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
    public class SectionTest : TestBase
    {
        public static Guid SectionId;

        [Test]
        public void CreateSectionTest()
        {
            var sectionName = RandomString();
            ResetWaiters();
            Section section = null;

            _commandService.Send(new CreateSection { SectionName = sectionName }, (result) =>
            {
                Assert.IsFalse(result.HasError);
                EventHandlerWaiter.WaitOne();
                section = _memoryCache.Get<Section>(SectionId.ToString());
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
            _commandService.Send(new ChangeSectionName { SectionId = SectionId, SectionName = newSectionName }, (result) =>
            {
                Assert.IsFalse(result.HasError);
                EventHandlerWaiter.WaitOne();
                section = _memoryCache.Get<Section>(SectionId.ToString());
                TestThreadWaiter.Set();
            });
            TestThreadWaiter.WaitOne(500);
            Assert.AreEqual(newSectionName, section.Name);
        }
    }
}
