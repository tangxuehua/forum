using System;
using Forum.Commands.Account;
using Forum.Commands.Post;
using Forum.Domain.Posts;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class PostTest : TestBase
    {
        public static Guid PostId;

        [Test]
        public void CreatePostTest()
        {
            var authorId = CreateAccountId();
            var subject = RandomString();
            var body = RandomString();
            var sectionId = Guid.NewGuid();
            ResetWaiters();
            Post post = null;

            CommandService.Send(new CreatePost { Subject = subject, Body = body, SectionId = sectionId, AuthorId = authorId }, result =>
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
        }
        [Test]
        public void ChangePostSubjectAndBodyTest()
        {
            CreatePostTest();

            ResetWaiters();
            var post = MemoryCache.Get<Post>(PostId.ToString());
            var subject = RandomString();
            var body = RandomString();

            CommandService.Send(new ChangePostSubjectAndBody { PostId = PostId, Subject = subject, Body = body }, result =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                post = MemoryCache.Get<Post>(PostId.ToString());
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.AreEqual(subject, post.Subject);
            Assert.AreEqual(body, post.Body);
        }

        protected Guid CreateAccountId()
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
