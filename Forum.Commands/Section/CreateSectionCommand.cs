using System;
using ENode.Commanding;

namespace Forum.Commands.Section
{
    [Serializable]
    public class CreateSectionCommand : Command<string>
    {
        public string Name { get; set; }

        public CreateSectionCommand(string id, string name) : base(id)
        {
            Name = name;
        }
    }
}
