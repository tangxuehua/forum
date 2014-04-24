using System;
using ENode.Eventing;

namespace Forum.Events.Section
{
    [Serializable]
    public class SectionNameChanged : DomainEvent<string>
    {
        public string Name { get; private set; }

        public SectionNameChanged(string sectionId, string name)
            : base(sectionId)
        {
            Name = name;
        }
    }
}
