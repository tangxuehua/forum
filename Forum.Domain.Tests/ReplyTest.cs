using System;
using Forum.Commands.Reply;
using Forum.Domain.Replies;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class ReplyTest : PostTest
    {
        public static Guid ReplyId;

        [Test]
        public void CreatePostReplyTest()
        {
            CreatePostTest();
            var body = RandomString();
            var authorId = CreateAccountId();
            ResetWaiters();
            Reply reply = null;

            CommandService.Send(new CreateReply { Body = body, PostId = PostId, AuthorId = authorId }, result =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                reply = MemoryCache.Get<Reply>(ReplyId);
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(reply);
            Assert.AreEqual(body, reply.Body);
            Assert.AreEqual(authorId, reply.AuthorId);
            Assert.AreEqual(PostId, reply.PostId);
        }
        [Test]
        public void CreateReplyReplyTest()
        {
            CreatePostReplyTest();
            var body = RandomString();
            var authorId = CreateAccountId();
            var parentId = ReplyId;
            ResetWaiters();
            Reply reply = null;

            CommandService.Send(new CreateReply { Body = body, ParentId = parentId, PostId = PostId, AuthorId = authorId }, result =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                reply = MemoryCache.Get<Reply>(ReplyId);
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(500);
            Assert.NotNull(reply);
            Assert.AreEqual(body, reply.Body);
            Assert.AreEqual(authorId, reply.AuthorId);
            Assert.AreEqual(PostId, reply.PostId);
            Assert.AreEqual(parentId, reply.ParentId);
        }
        [Test]
        public void ChangeReplyBodyTest()
        {
            CreatePostReplyTest();

            ResetWaiters();
            Reply reply = null;
            var body = RandomString();

            CommandService.Send(new ChangeReplyBody { ReplyId = ReplyId, Body = body }, result =>
            {
                Assert.IsNull(result.ErrorInfo);
                EventHandlerWaiter.WaitOne();
                reply = MemoryCache.Get<Reply>(ReplyId);
                TestThreadWaiter.Set();
            });

            TestThreadWaiter.WaitOne(5000000);
            Assert.AreEqual(body, reply.Body);
        }
    }
}
