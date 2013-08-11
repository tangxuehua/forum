using System;
using ENode.Commanding;

namespace Forum.Application.Commands
{
    [Serializable]
    public class ChangeSectionName : Command
    {
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
    }
}
