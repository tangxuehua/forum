using System;
using System.Collections.Generic;
using System.Linq;
using ENode.Commanding;
using ENode.EQueue;
using Forum.Commands.Accounts;

namespace Forum.EventService.Providers
{
    public class CommandTopicProvider : ICommandTopicProvider
    {
        private IDictionary<Type, string> _topicDict = new Dictionary<Type, string>();

        public CommandTopicProvider()
        {
            RegisterTopic("AccountCommandTopic",
                typeof(StartRegistrationCommand),
                typeof(ConfirmRegistrationCommand),
                typeof(CreateAccountCommand),
                typeof(CancelRegistrationCommand),
                typeof(CompleteRegistrationCommand));
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
