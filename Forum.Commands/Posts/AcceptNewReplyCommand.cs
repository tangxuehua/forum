using System;
using ENode.Commanding;

namespace Forum.Commands.Posts
{
    [Serializable]
    public class AcceptNewReplyCommand : Command
    {
        public string ReplyId { get; private set; }

        private AcceptNewReplyCommand() { }
        public AcceptNewReplyCommand(string id, string replyId)
            : base(id)
        {
            ReplyId = replyId;
        }
    }
}
