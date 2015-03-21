using System.Collections.Generic;
using System.Linq;
using ECommon.Components;
using ECommon.Extensions;
using ECommon.Utilities;
using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.QueryServices;
using Forum.QueryServices.DTOs;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class PostTest : TestBase
    {
        [Test]
        public void create_post_test()
        {
            var authorId = ObjectId.GenerateNewStringId();
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();

            var result = ExecuteCommand(new CreatePostCommand(subject, body, sectionId, authorId));

            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);

            var post = _postQueryService.Find(result.AggregateRootId);

            Assert.NotNull(post);
            Assert.AreEqual(subject, post.Subject);
            Assert.AreEqual(body, post.Body);
            Assert.AreEqual(authorId, post.AuthorId);
            Assert.AreEqual(sectionId, post.SectionId);
        }

        [Test]
        public void update_post_test()
        {
            var authorId = ObjectId.GenerateNewStringId();
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();

            var postId = ExecuteCommand(new CreatePostCommand(subject, body, sectionId, authorId)).AggregateRootId;

            var subject2 = ObjectId.GenerateNewStringId();
            var body2 = ObjectId.GenerateNewStringId();
            ExecuteCommand(new UpdatePostCommand(postId, subject2, body2));

            var post = _postQueryService.Find(postId);

            Assert.NotNull(post);
            Assert.AreEqual(subject2, post.Subject);
            Assert.AreEqual(body2, post.Body);
        }

        [Test]
        public void query_paged_post_test()
        {
            var authorId = ObjectId.GenerateNewStringId();
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();
            var postIds = new List<string>();
            var totalPostCount = 10;
            var replyCountPerPost = 2;
            var pageSize = 2;

            for (var i = 0; i < totalPostCount; i++)
            {
                var postId = ExecuteCommand(new CreatePostCommand(subject, body, sectionId, authorId)).AggregateRootId;
                for (var j = 0; j < replyCountPerPost; j++)
                {
                    ExecuteCommand(new CreateReplyCommand(postId, null, body, authorId));
                }
                postIds.Add(postId);
            }
            postIds.Reverse();

            var queryService = ObjectContainer.Resolve<IPostQueryService>();

            for (var pageIndex = 1; pageIndex <= totalPostCount / pageSize; pageIndex++)
            {
                var posts = queryService.Find(new PostQueryOption { SectionId = sectionId, PageInfo = new PageInfo { PageIndex = pageIndex, PageSize = pageSize } }).Posts.ToList();
                Assert.AreEqual(replyCountPerPost, posts.Count());
                var expectedPostIds = postIds.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                for (var i = 0; i < pageSize; i++)
                {
                    Assert.AreEqual(expectedPostIds[i], posts[i].Id);
                    Assert.AreEqual(replyCountPerPost, posts[i].ReplyCount);
                }
            }
        }

        [Test]
        public void query_post_detail_test()
        {
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();
            var authorName = ObjectId.GenerateNewStringId();
            var authorPassword = ObjectId.GenerateNewStringId();

            var authorId = ExecuteCommand(new RegisterNewAccountCommand(authorName, authorPassword)).AggregateRootId;
            var postId = ExecuteCommand(new CreatePostCommand(subject, body, sectionId, authorId)).AggregateRootId;
            ExecuteCommand(new CreateReplyCommand(postId, null, body, ObjectId.GenerateNewStringId()));
            var secondReplyId = ExecuteCommand(new CreateReplyCommand(postId, null, body, authorId)).AggregateRootId;

            var queryService = ObjectContainer.Resolve<IPostQueryService>();

            var post = queryService.Find(postId);
            Assert.IsNotNull(post);
            Assert.IsNotNull(post.ReplyList);
            Assert.AreEqual(2, post.ReplyCount);
            Assert.AreEqual(2, post.ReplyList.Count());
            Assert.AreEqual(authorId, post.MostRecentReplierId);
            Assert.AreEqual(authorName, post.MostRecentReplierName);
            Assert.AreEqual(secondReplyId, post.MostRecentReplyId);
            Assert.AreEqual(2, post.ReplyList.ToList()[1].Floor);
        }
    }
}
