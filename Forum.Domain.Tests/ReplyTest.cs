using ECommon.Extensions;
using ECommon.Utilities;
using ENode.Commanding;
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
            var postId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();

            var result = _commandService.Execute(new CreateReplyCommand(postId, null, body, authorId)).WaitResult<CommandResult>(3000);

            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);

            var reply = _memoryCache.Get<Reply>(result.AggregateRootId);

            Assert.NotNull(reply);
            Assert.AreEqual(result.AggregateRootId, reply.Id);
            Assert.AreEqual(postId, reply.PostId);
            Assert.AreEqual(authorId, reply.AuthorId);
            Assert.AreEqual(body, reply.Body);
        }

        [Test]
        public void create_reply_test2()
        {
            var postId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();

            var id1 = _commandService.Execute(new CreateReplyCommand(postId, null, body, authorId)).WaitResult<CommandResult>(3000).AggregateRootId;

            var body2 = ObjectId.GenerateNewStringId();

            var id2 = _commandService.Execute(new CreateReplyCommand(postId, id1, body2, authorId)).WaitResult<CommandResult>(3000).AggregateRootId;

            var reply = _memoryCache.Get<Reply>(id2);

            Assert.NotNull(reply);
            Assert.AreEqual(id2, reply.Id);
            Assert.AreEqual(postId, reply.PostId);
            Assert.AreEqual(authorId, reply.AuthorId);
            Assert.AreEqual(id1, reply.ParentId);
            Assert.AreEqual(body2, reply.Body);
        }

        [Test]
        public void update_reply_body_test()
        {
            var postId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();

            var id = _commandService.Execute(new CreateReplyCommand(postId, null, body, authorId)).WaitResult<CommandResult>(3000).AggregateRootId;

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
