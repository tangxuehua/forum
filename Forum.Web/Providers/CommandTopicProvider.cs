using System;
using System.Collections.Generic;
using System.Linq;
using ENode.Commanding;
using ENode.EQueue;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Commands.Sections;

namespace Forum.Web.Providers
{
    public class CommandTopicProvider : ICommandTopicProvider
    {
        private IDictionary<Type, string> _topicDict = new Dictionary<Type, string>();

        public CommandTopicProvider()
        {
            RegisterTopic("AccountCommandTopic", typeof(CreateAccountCommand));
            RegisterTopic("SectionCommandTopic", typeof(CreateSectionCommand), typeof(UpdateSectionCommand));
            RegisterTopic("PostCommandTopic", typeof(CreatePostCommand), typeof(UpdatePostCommand));
            RegisterTopic("ReplyCommandTopic", typeof(CreateReplyCommand), typeof(UpdateReplyBodyCommand));
        }

        public string GetTopic(ICommand command)
        {
            return _topicDict[command.GetType()];
        }
        public IEnumerable<string> GetAllCommandTopics()
        {
            return _topicDict.Values.Distinct();
        }

        private void RegisterTopic(string topic, params Type[] commandTypes)
        {
            foreach (var commandType in commandTypes)
            {
                _topicDict.Add(commandType, topic);
            }
        }
    }
}
