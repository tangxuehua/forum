using ECommon.Utilities;
using Forum.Commands.Posts;
using Forum.Domain.Posts;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class PostTest : TestBase
    {
        [Test]
        public void create_post_test()
        {
            var id = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreatePostCommand(id, subject, body, sectionId, authorId)).Wait();

            var post = _memoryCache.Get<Post>(id);

            Assert.NotNull(post);
            Assert.AreEqual(subject, post.Subject);
            Assert.AreEqual(body, post.Body);
            Assert.AreEqual(authorId, post.AuthorId);
            Assert.AreEqual(sectionId, post.SectionId);
        }
        [Test]
        public void update_post_test()
        {
            var id = ObjectId.GenerateNewStringId();
            var authorId = ObjectId.GenerateNewStringId();
            var subject = ObjectId.GenerateNewStringId();
            var body = ObjectId.GenerateNewStringId();
            var sectionId = ObjectId.GenerateNewStringId();

            _commandService.Execute(new CreatePostCommand(id, subject, body, sectionId, authorId)).Wait();

            var subject2 = ObjectId.GenerateNewStringId();
            var body2 = ObjectId.GenerateNewStringId();
            _commandService.Execute(new UpdatePostCommand(id, subject2, body2)).Wait();

            var post = _memoryCache.Get<Post>(id);

            Assert.NotNull(post);
            Assert.AreEqual(subject2, post.Subject);
            Assert.AreEqual(body2, post.Body);
        }
    }
}
