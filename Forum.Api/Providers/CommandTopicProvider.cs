using ENode.Commanding;
using ENode.EQueue;

namespace Forum.Api.Providers
{
    public class CommandTopicProvider : ICommandTopicProvider
    {
        public string GetTopic(ICommand command)
        {
            var fullName = command.GetType().FullName;
            return fullName.Substring(fullName.LastIndexOf('.') + 1) + "CommandTopic";
        }
    }
}
