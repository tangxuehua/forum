using System;
using ENode.Commanding;

namespace Forum.Commands.Sections
{
    [Serializable]
    public class ChangeSectionNameCommand : Command<string>
    {
        public string Name { get; set; }

        public ChangeSectionNameCommand(string id, string name) : base(id)
        {
            Name = name;
        }
    }
}
