using System;
using ENode.Commanding;
using ENode.Infrastructure;

namespace Forum.Commands.Replies
{
    [Code(15)]
    public class ChangeReplyBodyCommand : Command
    {
        public string Body { get; set; }

        private ChangeReplyBodyCommand() { }
        public ChangeReplyBodyCommand(string id, string body) : base(id)
        {
            Body = body;
        }
    }
}
