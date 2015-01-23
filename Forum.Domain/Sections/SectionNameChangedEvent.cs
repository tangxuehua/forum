using System;
using ENode.Eventing;

namespace Forum.Domain.Sections
{
    [Serializable]
    public class SectionNameChangedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }

        private SectionNameChangedEvent() { }
        public SectionNameChangedEvent(string sectionId, string name)
            : base(sectionId)
        {
            Name = name;
        }
    }
}
