using System;
using ENode.Commanding;

namespace Forum.Application.Commands.Reply
{
    [Serializable]
    public class CreateReply : Command
    {
        public Guid PostId { get; set; }
        public Guid? ParentId { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
    }
}
