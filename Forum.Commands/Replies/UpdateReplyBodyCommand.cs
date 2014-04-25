using System;
using ENode.Commanding;

namespace Forum.Commands.Replies
{
    [Serializable]
    public class UpdateReplyBodyCommand : Command<string>
    {
        public string Body { get; set; }

        public UpdateReplyBodyCommand(string id, string body) : base(id)
        {
            Body = body;
        }
    }
}
