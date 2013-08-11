using System;
using ENode.Commanding;
using Forum.Domain.Model;

namespace Forum.Application.Commands
{
    [Serializable]
    public class CreatePost : Command
    {
        public PostInfo PostInfo { get; set; }
    }
}
