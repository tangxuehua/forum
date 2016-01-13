using System;
using ENode.Commanding;
using ENode.Infrastructure;

namespace Forum.Commands.Sections
{
    public class CreateSectionCommand : Command
    {
        public string Name { get; set; }

        private CreateSectionCommand() { }
        public CreateSectionCommand(string id, string name) : base(id)
        {
            Name = name;
        }
    }
}
