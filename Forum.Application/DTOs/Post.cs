using System;
using System.Collections.Generic;

namespace Forum.Application.DTOs
{
    /// <summary>表示一个帖子及其所有的回复信息
    /// </summary>
    public class Post
    {
        /// <summary>唯一标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>所属版块
        /// </summary>
        public Guid SectionId { get; set; }
        /// <summary>作者ID
        /// </summary>
        public Guid AuthorId { get; set; }
        /// <summary>作者名称
        /// </summary>
        public string AuthorName { get; set; }
        /// <summary>创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>楼层，即第几个回复，根帖子的FloorIndex为0，第一个回复为1，以此类推；
        /// </summary>
        public int FloorIndex { get; set; }
        /// <summary>回复列表
        /// </summary>
        public IEnumerable<Post> ReplyList { get; set; }
    }
}
