using System;
using ENode.Commanding;

namespace Forum.Commands.Account
{
    [Serializable]
    public class CreateAccount : Command
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
