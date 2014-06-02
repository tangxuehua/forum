using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Web.Models
{
    public class PostListModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string SectionId { get; set; }
        public string CreatedOn { get; set; }
        public int ReplyCount { get; set; }
        public ReplyModel MostRecentReply { get; set; }
    }
    public class PostDetailModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string CreatedOn { get; set; }
        public int ReplyCount { get; set; }
        public IEnumerable<ReplyModel> Replies { get; set; }

        public PostDetailModel()
        {
            Replies = new List<ReplyModel>();
        }
    }
    public class CreatePostModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SectionId { get; set; }
    }
    public class EditPostModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}