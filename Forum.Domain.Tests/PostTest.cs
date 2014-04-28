using System.Collections.Generic;
using System.Linq;
using ECommon.Components;
using ECommon.Utilities;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Domain.Posts;
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
            var id = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreatePostCommand(id, subject, body, sectionId, authorId)).Wait();

            var post = _memoryCache.Get<Post>(id);

            Assert.NotNull(post);
            Assert.AreEqual(subject, post.Subject);
            Assert.AreEqual(body, post.Body);
            Assert.AreEqual(authorId, post.AuthorId);
            Assert.AreEqual(sectionId, post.SectionId);
        }
        [Test]
        public void update_post_test()
        {
            var id = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreatePostCommand(id, subject, body, sectionId, authorId)).Wait();

            var subject2 = ObjectId.GenerateNewStringId();
            var body2 = ObjectId.GenerateNewStringId();
            _commandService.Execute(new UpdatePostCommand(id, subject2, body2)).Wait();

            var post = _memoryCache.Get<Post>(id);

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
                var postId = ObjectId.GenerateNewStringId();
                _commandService.Execute(new CreatePostCommand(postId, subject, body, sectionId, authorId)).Wait();
                for (var j = 0; j < replyCountPerPost; j++)
                {
                    _commandService.Execute(new CreateReplyCommand(ObjectId.GenerateNewStringId(), postId, null, body, authorId)).Wait();
                }
                postIds.Add(postId);
            }

            var queryService = ObjectContainer.Resolve<IPostQueryService>();

            for (var pageIndex = 1; pageIndex <= totalPostCount / pageSize; pageIndex++)
            {
                var posts = queryService.QueryPosts(new PostQueryOption { SectionId = sectionId, PageInfo = new PageInfo { PageIndex = pageIndex, PageSize = pageSize } }).ToList();
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
            var postId = ObjectId.GenerateNewStringId();
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var authorName = ObjectId.GenerateNewStringId();
            var secondReplyId = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateAccountCommand(authorId, authorName, "123456")).Wait();

            _commandService.Execute(new CreatePostCommand(postId, subject, body, sectionId, authorId)).Wait();
            _commandService.Execute(new CreateReplyCommand(ObjectId.GenerateNewStringId(), postId, null, body, ObjectId.GenerateNewStringId())).Wait();
            _commandService.Execute(new CreateReplyCommand(secondReplyId, postId, null, body, authorId)).Wait();

            var queryService = ObjectContainer.Resolve<IPostQueryService>();

            var post = queryService.QueryPost(postId);
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
