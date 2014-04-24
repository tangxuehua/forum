using System;
using ENode.Commanding;

namespace Forum.Commands.Section
{
    [Serializable]
    public class CreateSection : Command<string>
    {
        public string SectionName { get; set; }

        public CreateSection(string sectionId, string sectionName) : base(sectionId)
        {
            SectionName = sectionName;
        }
    }
}
