using System;
using ENode.Commanding;
using ENode.Infrastructure;

namespace Forum.Commands.Replies
{
    public class CreateReplyCommand : Command
    {
        public string PostId { get; private set; }
        public string ParentId { get; private set; }
        public string Body { get; private set; }
        public string AuthorId { get; private set; }

        private CreateReplyCommand() { }
        public CreateReplyCommand(string id, string postId, string parentId, string body, string authorId) : base(id)
        {
            PostId = postId;
            ParentId = parentId;
            Body = body;
            AuthorId = authorId;
        }
    }
}
