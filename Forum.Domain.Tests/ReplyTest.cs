using ECommon.Utilities;
using Forum.Commands.Replies;
using Forum.Domain.Replies;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class ReplyTest : TestBase
    {
        [Test]
        public void create_reply_test1()
        {
            var id = ObjectId.GenerateNewStringId();
            var postId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateReplyCommand(id, postId, null, body, authorId)).Wait();

            var reply = _memoryCache.Get<Reply>(id);

            Assert.NotNull(reply);
            Assert.AreEqual(id, reply.Id);
            Assert.AreEqual(postId, reply.PostId);
            Assert.AreEqual(authorId, reply.AuthorId);
            Assert.AreEqual(body, reply.Body);
        }

        [Test]
        public void create_reply_test2()
        {
            var id = ObjectId.GenerateNewStringId();
            var postId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateReplyCommand(id, postId, null, body, authorId)).Wait();

            var id2 = ObjectId.GenerateNewStringId();
            var body2 = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateReplyCommand(id2, postId, id, body2, authorId)).Wait();

            var reply = _memoryCache.Get<Reply>(id2);

            Assert.NotNull(reply);
            Assert.AreEqual(id2, reply.Id);
            Assert.AreEqual(postId, reply.PostId);
            Assert.AreEqual(authorId, reply.AuthorId);
            Assert.AreEqual(body2, reply.Body);
        }

        [Test]
        public void update_reply_body_test()
        {
            var id = ObjectId.GenerateNewStringId();
            var postId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreateReplyCommand(id, postId, null, body, authorId)).Wait();

            var body2 = ObjectId.GenerateNewStringId();

            _commandService.Execute(new UpdateReplyBodyCommand(id, body2)).Wait();

            var reply = _memoryCache.Get<Reply>(id);

            Assert.NotNull(reply);
            Assert.AreEqual(id, reply.Id);
            Assert.AreEqual(postId, reply.PostId);
            Assert.AreEqual(authorId, reply.AuthorId);
            Assert.AreEqual(body2, reply.Body);
        }
    }
}
