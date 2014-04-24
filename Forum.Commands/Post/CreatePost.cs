using System;
using ENode.Commanding;

namespace Forum.Commands.Post
{
    [Serializable]
    public class CreatePost : Command<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string SectionId { get; private set; }
        public string AuthorId { get; private set; }

        public CreatePost(string postId, string subject, string body, string sectionId, string authorId) : base(postId)
        {
            Subject = subject;
            Body = body;
            SectionId = sectionId;
            AuthorId = authorId;
        }
    }
}
