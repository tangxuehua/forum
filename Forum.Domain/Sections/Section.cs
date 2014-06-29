using System;
using ENode.Domain;
using Forum.Infrastructure;

namespace Forum.Domain.Sections
{
    [Serializable]
    public class Section : AggregateRoot<string>
    {
        public string Name { get; private set; }

        public Section(string id, string name)
            : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("版块名称", name);
            RaiseEvent(new SectionCreatedEvent(Id, name));
        }

        public void ChangeName(string name)
        {
            RaiseEvent(new SectionNameChangedEvent(Id, name));
        }

        private void Handle(SectionCreatedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            Name = evnt.Name;
        }
        private void Handle(SectionNameChangedEvent evnt)
        {
            Name = evnt.Name;
        }
    }
}
