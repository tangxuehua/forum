using System;
using ENode.Commanding;

namespace Forum.Commands.Sections
{
    [Serializable]
    public class UpdateSectionCommand : Command<string>
    {
        public string Name { get; set; }

        public UpdateSectionCommand(string id, string name) : base(id)
        {
            Name = name;
        }
    }
}
