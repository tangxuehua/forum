using System;
using ENode.Commanding;

namespace Forum.Application.Commands
{
    [Serializable]
    public class CreateTopPost : Command
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public Guid SectionId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
