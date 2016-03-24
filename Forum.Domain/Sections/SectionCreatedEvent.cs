using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Sections
{
    public class SectionCreatedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        private SectionCreatedEvent() { }
        public SectionCreatedEvent(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
