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

            var result = _commandService.Execute(new CreateReplyCommand(postId, null, body, authorId), CommandReturnType.EventHandled).WaitResult<CommandResult>(10000);

            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);

            var reply = _replyQueryService.FindDynamic(result.AggregateRootId, "simple");

            Assert.NotNull(reply);
            Assert.AreEqual(result.AggregateRootId, reply.id);
            Assert.AreEqual(postId, reply.postId);
            Assert.AreEqual(authorId, reply.authorId);
            Assert.AreEqual(body, reply.body);
        }

        [Test]
        public void create_reply_test2()
        {
            var postId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();

            var id1 = _commandService.Execute(new CreateReplyCommand(postId, null, body, authorId), CommandReturnType.EventHandled).WaitResult<CommandResult>(10000).AggregateRootId;

            var body2 = ObjectId.GenerateNewStringId();

            var id2 = _commandService.Execute(new CreateReplyCommand(postId, id1, body2, authorId), CommandReturnType.EventHandled).WaitResult<CommandResult>(10000).AggregateRootId;

            var reply = _replyQueryService.FindDynamic(id2, "simple");

            Assert.NotNull(reply);
            Assert.AreEqual(id2, reply.id);
            Assert.AreEqual(postId, reply.postId);
            Assert.AreEqual(authorId, reply.authorId);
            Assert.AreEqual(id1, reply.parentId);
            Assert.AreEqual(body2, reply.body);
        }

        [Test]
        public void update_reply_body_test()
        {
            var postId = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();

            var id = _commandService.Execute(new CreateReplyCommand(postId, null, body, authorId), CommandReturnType.EventHandled).WaitResult<CommandResult>(10000).AggregateRootId;

            var body2 = ObjectId.GenerateNewStringId();

            _commandService.Execute(new ChangeReplyBodyCommand(id, body2), CommandReturnType.EventHandled).Wait();

            var reply = _replyQueryService.FindDynamic(id, "simple");

            Assert.NotNull(reply);
            Assert.AreEqual(id, reply.id);
            Assert.AreEqual(postId, reply.postId);
            Assert.AreEqual(authorId, reply.authorId);
            Assert.AreEqual(body2, reply.body);
        }
    }
}
