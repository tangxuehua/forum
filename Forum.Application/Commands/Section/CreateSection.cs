using System;
using ENode.Commanding;

namespace Forum.Application.Commands.Section
{
    [Serializable]
    public class CreateSection : Command
    {
        public string SectionName { get; set; }
    }
}
