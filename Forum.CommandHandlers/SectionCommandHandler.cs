using ECommon.IoC;
using ENode.Commanding;
using Forum.Commands.Section;
using Forum.Domain.Sections;

namespace Forum.CommandHandlers
{
    [Component]
    internal sealed class SectionCommandHandler :
        ICommandHandler<CreateSection>,
        ICommandHandler<ChangeSectionName>
    {
        public void Handle(ICommandContext context, CreateSection command)
        {
            context.Add(new Section(command.AggregateRootId, command.SectionName));
        }
        public void Handle(ICommandContext context, ChangeSectionName command)
        {
            context.Get<Section>(command.AggregateRootId).ChangeName(command.SectionName);
        }
    }
}
