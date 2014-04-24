using System;
using ENode.Commanding;

namespace Forum.Commands.Reply
{
    [Serializable]
    public class UpdateReplyCommand : Command<string>
    {
        public string Body { get; set; }

        public UpdateReplyCommand(string id, string body) : base(id)
        {
            Body = body;
        }
    }
}
