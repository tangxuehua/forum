using System;
using ENode.Eventing;

namespace Forum.Events.Section
{
    [Serializable]
    public class SectionCreated : Event
    {
        public Guid SectionId { get; private set; }
        public string Name { get; private set; }

        public SectionCreated(Guid sectionId, string name)
        {
            SectionId = sectionId;
            Name = name;
        }
    }
}
