using System;
using ENode.Commanding;

namespace Forum.Commands.Section
{
    [Serializable]
    public class ChangeSectionName : Command<string>
    {
        public string SectionName { get; set; }

        public ChangeSectionName(string sectionId, string sectionName) : base(sectionId)
        {
            SectionName = sectionName;
        }
    }
}
