using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Commands.Sections;

namespace Forum.Tests
{
    [Component]
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public CommandTopicProvider()
        {
            RegisterTopic("AccountCommandTopic", typeof(RegisterNewAccountCommand));
            RegisterTopic("SectionCommandTopic", typeof(CreateSectionCommand), typeof(ChangeSectionCommand));
            RegisterTopic("PostCommandTopic", typeof(CreatePostCommand), typeof(UpdatePostCommand), typeof(AcceptNewReplyCommand));
            RegisterTopic("ReplyCommandTopic", typeof(CreateReplyCommand), typeof(ChangeReplyBodyCommand));
        }
    }
}
