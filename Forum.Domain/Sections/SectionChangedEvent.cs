using ENode.Eventing;

namespace Forum.Domain.Sections
{
    public class SectionChangedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private SectionChangedEvent() { }
        public SectionChangedEvent(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
