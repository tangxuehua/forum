using System;
using ENode.Commanding;

namespace Forum.Application.Commands
{
    [Serializable]
    public class CreateSection : Command
    {
        public string SectionName { get; set; }
    }
}
