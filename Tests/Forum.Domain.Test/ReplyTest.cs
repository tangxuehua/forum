using System;
using Forum.Application.Commands.Reply;
using Forum.Domain.Replies.Model;
using NUnit.Framework;

namespace Forum.Domain.Test
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
            var postId = PostId;
            ResetWaiters();
            Reply reply = null;

            CommandService.Send(new CreateReply { Body = body, PostId = postId, AuthorId = authorId }, result =>
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
            Assert.AreEqual(postId, reply.PostId);
        }
        [Test]
        public void ChangePostBodyTest()
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
