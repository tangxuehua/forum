using System;
using ENode.Commanding;

namespace Forum.Commands.Post
{
    [Serializable]
    public class ChangePostSubjectAndBody : Command<string>
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public ChangePostSubjectAndBody(string postId, string subject, string body) : base(postId)
        {
            Subject = subject;
            Body = body;
        }
    }
}
