using ENode.Commanding;

namespace Forum.Commands.Sections
{
    public class ChangeSectionCommand : Command
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private ChangeSectionCommand() { }
        public ChangeSectionCommand(string id, string name,string description) : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
