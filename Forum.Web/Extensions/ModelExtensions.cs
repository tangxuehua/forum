using Forum.QueryServices.DTOs;
using Forum.Web.Models;

namespace Forum.Web.Extensions
{
    public static class ModelExtensions
    {
        public static PostListModel ToViewModel(this PostInfo postInfo)
        {
            var model = new PostListModel();
            model.Id = postInfo.Id;
            model.Subject = postInfo.Subject;
            model.SectionId = postInfo.SectionId;
            model.AuthorId = postInfo.AuthorId;
            model.AuthorName = string.IsNullOrEmpty(postInfo.AuthorName) ? postInfo.AuthorId : postInfo.AuthorName;
            model.CreatedOn = postInfo.CreatedOn.ToString("MM-dd hh:mm");
            model.ReplyCount = postInfo.ReplyCount;
            if (!string.IsNullOrEmpty(postInfo.MostRecentReplyId))
            {
                model.MostRecentReply = new ReplyModel
                {
                    Id = postInfo.MostRecentReplyId,
                    AuthorId = postInfo.MostRecentReplierId,
                    AuthorName = string.IsNullOrEmpty(postInfo.MostRecentReplierName) ? postInfo.MostRecentReplierId : postInfo.MostRecentReplierName,
                    CreatedOn = postInfo.MostRecentReplyCreatedOn.ToString("MM-dd hh:mm")
                };
            }

            return model;
        }
    }
}