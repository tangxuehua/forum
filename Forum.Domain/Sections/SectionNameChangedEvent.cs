using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Sections
{
    [Code(17)]
    public class SectionNameChangedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }

        private SectionNameChangedEvent() { }
        public SectionNameChangedEvent(Section section, string name)
            : base(section)
        {
            Name = name;
        }
    }
}
