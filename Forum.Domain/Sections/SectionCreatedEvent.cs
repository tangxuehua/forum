using System;
using ENode.Eventing;

namespace Forum.Domain.Sections
{
    [Serializable]
    public class SectionCreatedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }

        private SectionCreatedEvent() { }
        public SectionCreatedEvent(Section section, string name)
            : base(section)
        {
            Name = name;
        }
    }
}
