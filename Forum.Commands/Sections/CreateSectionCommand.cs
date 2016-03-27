using ENode.Commanding;

namespace Forum.Commands.Sections
{
    public class CreateSectionCommand : Command
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private CreateSectionCommand() { }
        public CreateSectionCommand(string id, string name, string description)
            : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
