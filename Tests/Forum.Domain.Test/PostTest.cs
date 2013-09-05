using System;
using Forum.Application.Commands;
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

            CommandService.Send(new CreatePost { PostInfo = postInfo }, (result) =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                post = MemoryCache.Get<Post>(PostId.ToString());
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

            CommandService.Send(new CreatePost { PostInfo = postInfo }, (result) =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                post = MemoryCache.Get<Post>(PostId.ToString());
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
            Assert.NotNull(postInfo.RootId);
            Assert.AreEqual(postInfo.RootId.Value, post.RootId);
        }
        [Test]
        public void ChangePostBodyTest()
        {
            CreateTopPostTest();

            ResetWaiters();
            var post = MemoryCache.Get<Post>(PostId.ToString());
            var body = RandomString();

            CommandService.Send(new ChangePostBody { PostId = PostId, Body = body }, (result) =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                post = MemoryCache.Get<Post>(PostId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.AreEqual(body, post.Body);
        }

        private Guid CreateAccountId()
        {
            ResetWaiters();
            Guid? authorId = null;

            CommandService.Send(new CreateAccount { Name = RandomString(), Password = RandomString() }, (result) =>
            {
                Assert.IsNull(result.ErrorInfo);
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
