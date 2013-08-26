using System;
using ENode.Commanding;

namespace Forum.Application.Commands
{
    [Serializable]
    public class ChangePostBody : Command
    {
        public Guid PostId { get; set; }
        public string Body { get; set; }
    }
}
