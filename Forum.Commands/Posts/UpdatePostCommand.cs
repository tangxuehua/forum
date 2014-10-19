using System;
using ENode.Commanding;

namespace Forum.Commands.Posts
{
    [Serializable]
    public class UpdatePostCommand : AggregateCommand<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public UpdatePostCommand(string id, string subject, string body) : base(id)
        {
            Subject = subject;
            Body = body;
        }
    }
}
