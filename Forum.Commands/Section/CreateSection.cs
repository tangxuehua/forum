using System;
using ENode.Commanding;

namespace Forum.Commands.Section
{
    [Serializable]
    public class CreateSection : Command
    {
        public string SectionName { get; set; }
    }
}
