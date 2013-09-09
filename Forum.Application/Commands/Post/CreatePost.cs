using System;
using ENode.Commanding;

namespace Forum.Application.Commands.Post
{
    [Serializable]
    public class CreatePost : Command
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public Guid SectionId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
