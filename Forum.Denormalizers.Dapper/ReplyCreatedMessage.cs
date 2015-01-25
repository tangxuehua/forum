using System;
using ENode.Messaging;

namespace Forum.Denormalizers.Dapper
{
    [Serializable]
    public class ReplyCreatedMessage : VersionedMessage<string>
    {
        public string PostId { get; set; }
        public string ParentId { get; set; }
        public string AuthorId { get; set; }
    }
}
