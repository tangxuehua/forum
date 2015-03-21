using System;
using System.Collections.Generic;
using ENode.Domain;
using Forum.Domain.Replies;
using Forum.Infrastructure;

namespace Forum.Domain.Posts
{
    /// <summary>帖子聚合根
    /// </summary>
    [Serializable]
    public class Post : AggregateRoot<string>
    {
        private string _subject;
        private string _body;
        private string _sectionId;
        private string _authorId;
        private ISet<string> _replyIds;
        private PostReplyStatisticInfo _replyStatisticInfo;

        public Post(string id, string subject, string body, string sectionId, string authorId)
            : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("帖子标题", subject);
            Assert.IsNotNullOrWhiteSpace("帖子内容", body);
            Assert.IsNotNullOrWhiteSpace("帖子所属版块", sectionId);
            Assert.IsNotNullOrWhiteSpace("帖子作者", authorId);
            if (subject.Length > 256)
            {
                throw new Exception("帖子标题长度不能超过256");
            }
            if (body.Length > 1000)
            {
                throw new Exception("帖子内容长度不能超过1000");
            }
            ApplyEvent(new PostCreatedEvent(this, subject, body, sectionId, authorId));
        }

        public void Update(string subject, string body)
        {
            Assert.IsNotNullOrWhiteSpace("帖子标题", subject);
            Assert.IsNotNullOrWhiteSpace("帖子内容", body);
            if (subject.Length > 256)
            {
                throw new Exception("帖子标题长度不能超过256");
            }
            if (body.Length > 1000)
            {
                throw new Exception("帖子内容长度不能超过1000");
            }
            ApplyEvent(new PostUpdatedEvent(this, subject, body));
        }
        public void AcceptNewReply(Reply reply)
        {
            if (!_replyIds.Add(reply.Id)) return;

            if (_replyStatisticInfo == null)
            {
                ApplyEvent(new PostReplyStatisticInfoChangedEvent(this, new PostReplyStatisticInfo(
                    reply.Id,
                    reply.GetAuthorId(),
                    reply.GetCreateTime(),
                    1)));
            }
            else if (_replyStatisticInfo.LastReplyTime < reply.GetCreateTime())
            {
                ApplyEvent(new PostReplyStatisticInfoChangedEvent(this, new PostReplyStatisticInfo(
                    reply.Id,
                    reply.GetAuthorId(),
                    reply.GetCreateTime(),
                    _replyStatisticInfo.ReplyCount + 1)));
            }
            else
            {
                ApplyEvent(new PostReplyStatisticInfoChangedEvent(this, new PostReplyStatisticInfo(
                    _replyStatisticInfo.LastReplyId,
                    _replyStatisticInfo.LastReplyAuthorId,
                    _replyStatisticInfo.LastReplyTime,
                    _replyStatisticInfo.ReplyCount + 1)));
            }
        }

        private void Handle(PostCreatedEvent evnt)
        {
            _replyIds = new HashSet<string>();
            _id = evnt.AggregateRootId;
            _subject = evnt.Subject;
            _body = evnt.Body;
            _sectionId = evnt.SectionId;
            _authorId = evnt.AuthorId;
        }
        private void Handle(PostUpdatedEvent evnt)
        {
            _subject = evnt.Subject;
            _body = evnt.Body;
        }
        private void Handle(PostReplyStatisticInfoChangedEvent evnt)
        {
            _replyStatisticInfo = evnt.StatisticInfo;
        }
    }
}