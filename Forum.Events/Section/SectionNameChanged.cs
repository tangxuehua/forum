using System;
using ENode.Eventing;

namespace Forum.Events.Section
{
    [Serializable]
    public class SectionNameChanged : Event
    {
        public Guid SectionId { get; private set; }
        public string Name { get; private set; }

        public SectionNameChanged(Guid sectionId, string name)
        {
            SectionId = sectionId;
            Name = name;
        }
    }
}
