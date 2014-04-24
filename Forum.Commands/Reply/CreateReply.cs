using System;
using ENode.Commanding;

namespace Forum.Commands.Reply
{
    [Serializable]
    public class CreateReply : Command<string>
    {
        public string PostId { get; private set; }
        public string ParentId { get; private set; }
        public string Body { get; private set; }
        public string AuthorId { get; private set; }

        public CreateReply(string replyId, string postId, string parentId, string body, string authorId) : base(replyId)
        {
            PostId = postId;
            ParentId = parentId;
            Body = body;
            AuthorId = authorId;
        }
    }
}
