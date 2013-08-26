using System;

namespace Forum.Domain.Model
{
    [Serializable]
    public class PostInfo
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public Guid? ParentId { get; private set; }
        public Guid? RootId { get; private set; }
        public Guid SectionId { get; private set; }
        public Guid AuthorId { get; private set; }

        public PostInfo(string subject, string body, Guid? parentId, Guid? rootId, Guid sectionId, Guid authorId)
        {
            if (parentId == null && string.IsNullOrWhiteSpace(subject))
            {
                throw new Exception("帖子标题不能为空");
            }
            if (parentId != null && rootId == null)
            {
                throw new Exception("帖子ID不能为空.");
            }
            Subject = subject;
            Body = body;
            ParentId = parentId;
            RootId = rootId;
            SectionId = sectionId;
            AuthorId = authorId;
        }
    }
}
