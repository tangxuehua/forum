using System.Threading;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Forum.Tests
{
    [TestClass]
    public class ReplyTest : TestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Initialize();
        }

        [TestMethod]
        public void create_reply_test()
        {
            //创建账号
            var name = ObjectId.GenerateNewStringId();
            var password = ObjectId.GenerateNewStringId();
            var result = ExecuteCommand(new RegisterNewAccountCommand(ObjectId.GenerateNewStringId(), name, password));
            Assert.AreEqual(CommandStatus.Success, result.Status);

            //发表帖子
            var authorId = result.AggregateRootId;
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();
            result = ExecuteCommand(new CreatePostCommand(ObjectId.GenerateNewStringId(), subject, body, sectionId, authorId));
            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);

            //发表回复
            var postId = result.AggregateRootId;
            result = ExecuteCommand(new CreateReplyCommand(ObjectId.GenerateNewStringId(), postId, null, body, authorId));

            //验证回复信息
            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);
            var replyId = result.AggregateRootId;
            var reply = _replyQueryService.FindDynamic(replyId, "simple");
            Assert.IsNotNull(reply);
            Assert.AreEqual(replyId, reply.id);
            Assert.AreEqual(postId, reply.postId);
            Assert.AreEqual(authorId, reply.authorId);
            Assert.AreEqual(body, reply.body);

            //停顿3s后验证帖子统计信息
            Thread.Sleep(3000);
            var postInfo = _postQueryService.Find(postId);
            Assert.IsNotNull(postInfo);
            Assert.AreEqual(replyId, postInfo.MostRecentReplyId);
            Assert.AreEqual(authorId, postInfo.MostRecentReplierId);
            Assert.AreEqual(name, postInfo.MostRecentReplierName);
            Assert.AreEqual(reply.createdOn, postInfo.LastUpdateTime);
            Assert.AreEqual(1, postInfo.ReplyCount);

            //再次发表回复
            var parentId = result.AggregateRootId;
            result = ExecuteCommand(new CreateReplyCommand(ObjectId.GenerateNewStringId(), postId, parentId, body, authorId));

            //验证回复信息
            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);
            replyId = result.AggregateRootId;
            reply = _replyQueryService.FindDynamic(replyId, "simple");
            Assert.IsNotNull(reply);
            Assert.AreEqual(replyId, reply.id);
            Assert.AreEqual(postId, reply.postId);
            Assert.AreEqual(authorId, reply.authorId);
            Assert.AreEqual(parentId, reply.parentId);
            Assert.AreEqual(body, reply.body);

            //停顿3s后验证帖子统计信息
            Thread.Sleep(3000);
            postInfo = _postQueryService.Find(postId);
            Assert.IsNotNull(postInfo);
            Assert.AreEqual(replyId, postInfo.MostRecentReplyId);
            Assert.AreEqual(authorId, postInfo.MostRecentReplierId);
            Assert.AreEqual(name, postInfo.MostRecentReplierName);
            Assert.AreEqual(reply.createdOn, postInfo.LastUpdateTime);
            Assert.AreEqual(2, postInfo.ReplyCount);
        }
        [TestMethod]
        public void update_reply_test()
        {
            //创建账号
            var name = ObjectId.GenerateNewStringId();
            var password = ObjectId.GenerateNewStringId();
            var result = ExecuteCommand(new RegisterNewAccountCommand(ObjectId.GenerateNewStringId(), name, password));
            Assert.AreEqual(CommandStatus.Success, result.Status);

            //发表帖子
            var authorId = result.AggregateRootId;
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();
            result = ExecuteCommand(new CreatePostCommand(ObjectId.GenerateNewStringId(), subject, body, sectionId, authorId));
            Assert.AreEqual(CommandStatus.Success, result.Status);
            Assert.IsNotNull(result.AggregateRootId);

            //发表回复
            var postId = result.AggregateRootId;
            result = ExecuteCommand(new CreateReplyCommand(ObjectId.GenerateNewStringId(), postId, null, body, authorId));

            //修改回复
            var body2 = ObjectId.GenerateNewStringId();
            ExecuteCommand(new ChangeReplyBodyCommand(result.AggregateRootId, body2));

            var reply = _replyQueryService.FindDynamic(result.AggregateRootId, "simple");
            Assert.IsNotNull(reply);
            Assert.AreEqual(result.AggregateRootId, reply.id);
            Assert.AreEqual(postId, reply.postId);
            Assert.AreEqual(authorId, reply.authorId);
            Assert.AreEqual(body2, reply.body);
        }
    }
}
