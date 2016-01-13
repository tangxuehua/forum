using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Sections
{
    public class SectionNameChangedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }

        private SectionNameChangedEvent() { }
        public SectionNameChangedEvent(string name)
        {
            Name = name;
        }
    }
}
