using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models
{
    public class ReplyModel
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string CreatedOn { get; set; }
        public int Floor { get; set; }
    }
    public class CreateReplyModel
    {
        [Required(ErrorMessage = "请输入回复内容。")]
        public string Body { get; set; }
        [Required(ErrorMessage = "回复对应的帖子ID不能为空。")]
        public string PostId { get; set; }
        public string ParentId { get; set; }
    }
    public class EditReplyModel
    {
        [Required(ErrorMessage = "回复ID不能为空。")]
        public string Id { get; set; }
        [Required(ErrorMessage = "回复作者ID不能为空。")]
        public string AuthorId { get; set; }
        [Required(ErrorMessage = "请输入回复内容。")]
        public string Body { get; set; }
    }
}