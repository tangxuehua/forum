using System;
using ENode.Commanding;

namespace Forum.Commands.Replies
{
    [Serializable]
    public class ChangeReplyBodyCommand : AggregateCommand<string>
    {
        public string Body { get; set; }

        private ChangeReplyBodyCommand() { }
        public ChangeReplyBodyCommand(string id, string body) : base(id)
        {
            Body = body;
        }
    }
}
