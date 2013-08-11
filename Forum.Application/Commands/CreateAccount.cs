using System;
using ENode.Commanding;

namespace Forum.Application.Commands
{
    [Serializable]
    public class CreateAccount : Command
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
