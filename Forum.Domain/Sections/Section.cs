using System;
using ENode.Domain;
using Forum.Events.Section;

namespace Forum.Domain.Sections
{
    [Serializable]
    public class Section : AggregateRoot<string>
    {
        public string Name { get; private set; }

        public Section(string id, string name) : base(id)
        {
            RaiseEvent(new SectionCreated(Id, name));
        }

        public void Update(string name)
        {
            RaiseEvent(new SectionNameChanged(Id, name));
        }

        private void Handle(SectionCreated evnt)
        {
            Id = evnt.AggregateRootId;
            Name = evnt.Name;
        }
        private void Handle(SectionNameChanged evnt)
        {
            Name = evnt.Name;
        }
    }
}
