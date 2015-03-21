using System;
using ENode.Eventing;

namespace Forum.Domain.Sections
{
    [Serializable]
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
