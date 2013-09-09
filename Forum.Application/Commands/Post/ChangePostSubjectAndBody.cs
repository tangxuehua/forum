using System;
using ENode.Commanding;

namespace Forum.Application.Commands.Post
{
    [Serializable]
    public class ChangePostSubjectAndBody : Command
    {
        public Guid PostId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
