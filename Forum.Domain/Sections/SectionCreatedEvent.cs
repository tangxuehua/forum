using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Sections
{
    public class SectionCreatedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }

        private SectionCreatedEvent() { }
        public SectionCreatedEvent(string name)
        {
            Name = name;
        }
    }
}
