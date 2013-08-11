using System;
using ENode.Eventing;
using ENode.Infrastructure;
using Forum.Application.Commands;
using Forum.Domain.Events;
using Forum.Domain.Model;
using NUnit.Framework;

namespace Forum.Domain.Test
{
    [TestFixture]
    public class PostTest : TestBase
    {
        public static Guid PostId;
        public static Guid ParentId;

        [Test]
        public void CreateTopPostTest()
        {
            var authorId = CreateAccountId();
            var postInfo = new PostInfo(RandomString(), RandomString(), null, null, Guid.NewGuid(), authorId);
            ResetWaiters();
            Post post = null;

            _commandService.Send(new CreatePost { PostInfo = postInfo }, (result) =>
            {
                Assert.IsFalse(result.HasError);
                EventHandlerWaiter.WaitOne();
                post = _memoryCache.Get<Post>(PostId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(post);
            Assert.AreEqual(postInfo.Subject, post.Subject);
            Assert.AreEqual(postInfo.Body, post.Body);
            Assert.AreEqual(postInfo.AuthorId, post.AuthorId);
            Assert.AreEqual(postInfo.SectionId, post.SectionId);
            Assert.AreEqual(postInfo.ParentId, post.ParentId);
            Assert.AreEqual(post.Id, post.RootId);
        }
        [Test]
        public void CreateReplyPostTest()
        {
            CreateTopPostTest();
            var authorId = CreateAccountId();
            var postInfo = new PostInfo(null, RandomString(), PostId, PostId, Guid.NewGuid(), authorId);
            ResetWaiters();
            PostId = Guid.Empty;
            Post post = null;

            _commandService.Send(new CreatePost { PostInfo = postInfo }, (result) =>
            {
                Assert.IsFalse(result.HasError);
                EventHandlerWaiter.WaitOne();
                post = _memoryCache.Get<Post>(PostId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(post);
            Assert.NotNull(post.ParentId);
            Assert.AreEqual(postInfo.Subject, post.Subject);
            Assert.AreEqual(postInfo.Body, post.Body);
            Assert.AreEqual(postInfo.AuthorId, post.AuthorId);
            Assert.AreEqual(postInfo.SectionId, post.SectionId);
            Assert.AreEqual(postInfo.ParentId, post.ParentId);
            Assert.AreEqual(postInfo.RootId.Value, post.RootId);
        }
        [Test]
        public void ChangePostBodyTest()
        {
            CreateTopPostTest();

            ResetWaiters();
            var post = _memoryCache.Get<Post>(PostId.ToString());
            var body = RandomString();

            _commandService.Send(new ChangePostBody { PostId = PostId, Body = body }, (result) =>
            {
                Assert.IsFalse(result.HasError);
                EventHandlerWaiter.WaitOne();
                post = _memoryCache.Get<Post>(PostId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.AreEqual(body, post.Body);
        }

        private Guid CreateAccountId() {
            ResetWaiters();
            Guid? authorId = null;

            _commandService.Send(new CreateAccount { Name = RandomString(), Password = RandomString() }, (result) => {
                Assert.IsFalse(result.HasError);
                EventHandlerWaiter.WaitOne();
                authorId = AccountTest.AccountId;
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(authorId);
            return authorId.Value;
        }
    }
}
