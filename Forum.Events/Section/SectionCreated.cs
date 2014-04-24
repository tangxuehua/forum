using System;
using ENode.Eventing;

namespace Forum.Events.Section
{
    [Serializable]
    public class SectionCreated : DomainEvent<string>
    {
        public string Name { get; private set; }

        public SectionCreated(string sectionId, string name)
            : base(sectionId)
        {
            Name = name;
        }
    }
}
