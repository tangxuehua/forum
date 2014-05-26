using System;
using ENode.Commanding;

namespace Forum.Commands.Sections
{
    [Serializable]
    public class CreateSectionCommand : Command<string>, ICreatingAggregateCommand
    {
        public string Name { get; set; }

        public CreateSectionCommand(string name)
        {
            Name = name;
        }
    }
}
