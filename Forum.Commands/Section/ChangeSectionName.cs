using System;
using ENode.Commanding;

namespace Forum.Commands.Section
{
    [Serializable]
    public class ChangeSectionName : Command
    {
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
    }
}
