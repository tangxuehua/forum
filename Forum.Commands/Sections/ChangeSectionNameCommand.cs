using System;
using ENode.Commanding;
using ENode.Infrastructure;

namespace Forum.Commands.Sections
{
    public class ChangeSectionNameCommand : Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        private ChangeSectionNameCommand() { }
        public ChangeSectionNameCommand(string id, string name,string description) : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
