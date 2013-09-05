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

        [Test]
        public void CreateTopPostTest()
        {
            var authorId = CreateAccountId();
            var subject = RandomString();
            var body = RandomString();
            var sectionId = Guid.NewGuid();
            ResetWaiters();
            Post post = null;

            CommandService.Send(new CreateTopPost { Subject = subject, Body = body, SectionId = sectionId, AuthorId = authorId }, result =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                post = MemoryCache.Get<Post>(PostId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(post);
            Assert.AreEqual(subject, post.Subject);
            Assert.AreEqual(body, post.Body);
            Assert.AreEqual(authorId, post.AuthorId);
            Assert.AreEqual(sectionId, post.SectionId);
            Assert.IsNull(post.ParentId);
            Assert.AreEqual(post.Id, post.RootId);
        }
        [Test]
        public void CreateReplyPostTest()
        {
            CreateTopPostTest();
            var authorId = CreateAccountId();
            var body = RandomString();
            var sectionId = Guid.NewGuid();
            var parentId = PostId;
            var rootId = PostId;
            ResetWaiters();
            PostId = Guid.Empty;
            Post post = null;

            CommandService.Send(new CreateReplyPost { Body = body, ParentId = parentId, RootId = rootId, SectionId = sectionId, AuthorId = authorId }, result =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                post = MemoryCache.Get<Post>(PostId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(post);
            Assert.NotNull(post.ParentId);
            Assert.IsNull(post.Subject);
            Assert.AreEqual(body, post.Body);
            Assert.AreEqual(authorId, post.AuthorId);
            Assert.AreEqual(sectionId, post.SectionId);
            Assert.AreEqual(parentId, post.ParentId);
            Assert.AreEqual(rootId, post.RootId);
        }
        [Test]
        public void ChangePostBodyTest()
        {
            CreateTopPostTest();

            ResetWaiters();
            var post = MemoryCache.Get<Post>(PostId.ToString());
            var body = RandomString();

            CommandService.Send(new ChangePostBody { PostId = PostId, Body = body }, result =>
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

            CommandService.Send(new CreateAccount { Name = RandomString(), Password = RandomString() }, result =>
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
