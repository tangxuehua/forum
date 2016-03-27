using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string LastUpdateTime { get; set; }
        public ReplyModel MostRecentReply { get; set; }
        public string SectionName { get; set; }
    }
    public class PostDetailModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string CreatedOn { get; set; }
        public int ReplyCount { get; set; }
        public string SectionId { get; set; }
        public string SectionName { get; set; }
        public IEnumerable<ReplyModel> Replies { get; set; }

        public PostDetailModel()
        {
            Replies = new List<ReplyModel>();
        }
    }
    public class CreatePostModel
    {
        [Required(ErrorMessage = "请输入帖子标题。")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "请输入帖子内容。")]
        public string Body { get; set; }
        [Required(ErrorMessage = "请选择帖子所属版块。")]
        public string SectionId { get; set; }
    }
    public class EditPostModel
    {
        [Required(ErrorMessage = "帖子ID不能为空。")]
        public string Id { get; set; }
        [Required(ErrorMessage = "帖子作者ID不能为空。")]
        public string AuthorId { get; set; }
        [Required(ErrorMessage = "请输入帖子标题。")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "请输入帖子内容。")]
        public string Body { get; set; }
    }
}