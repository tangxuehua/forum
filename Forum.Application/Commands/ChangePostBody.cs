using System;
using ENode.Commanding;
using Forum.Domain.Model;

namespace Forum.Application.Commands
{
    [Serializable]
    public class ChangePostBody : Command
    {
        public Guid PostId { get; set; }
        public string Body { get; set; }
    }
}
