using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Sections
{
    [Code(16)]
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
