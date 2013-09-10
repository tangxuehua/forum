using System;
using ENode.Commanding;

namespace Forum.Commands.Reply
{
    [Serializable]
    public class ChangeReplyBody : Command
    {
        public Guid ReplyId { get; set; }
        public string Body { get; set; }
    }
}
