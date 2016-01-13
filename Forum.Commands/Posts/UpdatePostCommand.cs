using System;
using ENode.Commanding;
using ENode.Infrastructure;

namespace Forum.Commands.Posts
{
    public class UpdatePostCommand : Command
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }

        private UpdatePostCommand() { }
        public UpdatePostCommand(string id, string subject, string body) : base(id)
        {
            Subject = subject;
            Body = body;
        }
    }
}
