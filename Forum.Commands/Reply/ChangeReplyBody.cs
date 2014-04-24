using System;
using ENode.Commanding;

namespace Forum.Commands.Reply
{
    [Serializable]
    public class ChangeReplyBody : Command<string>
    {
        public string Body { get; set; }

        public ChangeReplyBody(string replyId, string body) : base(replyId)
        {
            Body = body;
        }
    }
}
