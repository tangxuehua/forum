using ENode.Eventing;

namespace Forum.Domain.Sections
{
    public class SectionCreatedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private SectionCreatedEvent() { }
        public SectionCreatedEvent(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
