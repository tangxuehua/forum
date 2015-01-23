using System;
using ENode.Commanding;

namespace Forum.Commands.Sections
{
    [Serializable]
    public class CreateSectionCommand : AggregateCommand<string>, ICreatingAggregateCommand
    {
        public string Name { get; set; }

        private CreateSectionCommand() { }
        public CreateSectionCommand(string name)
        {
            Name = name;
        }
    }
}
