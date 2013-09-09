using System;
using ENode.Eventing;

namespace Forum.Domain.Posts.Events
{
    [Serializable]
    public class PostSubjectAndBodyChanged : Event
    {
        public Guid PostId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public PostSubjectAndBodyChanged(Guid postId, string subject, string body)
        {
            PostId = postId;
            Subject = subject;
            Body = body;
        }
    }
}
