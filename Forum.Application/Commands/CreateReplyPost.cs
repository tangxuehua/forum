using System;
using ENode.Commanding;

namespace Forum.Application.Commands
{
    [Serializable]
    public class CreateReplyPost : Command
    {
        public Guid ParentId { get; set; }
        public Guid RootId { get; set; }
        public string Body { get; set; }
        public Guid SectionId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
