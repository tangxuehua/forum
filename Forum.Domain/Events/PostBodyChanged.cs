using System;
using ENode.Eventing;

namespace Forum.Domain.Events
{
    [Serializable]
    public class PostBodyChanged : Event
    {
        public Guid PostId { get; private set; }
        public Guid? ParentId { get; private set; }
        public Guid RootId { get; private set; }
        public string Body { get; private set; }

        public PostBodyChanged(Guid postId, Guid? parentId, Guid rootId, string body)
        {
            PostId = postId;
            ParentId = parentId;
            RootId = rootId;
            Body = body;
        }
    }
}
