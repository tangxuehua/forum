using ECommon.IoC;
using ENode.Commanding;
using Forum.Commands.Section;
using Forum.Domain.Sections;

namespace Forum.CommandHandlers
{
    [Component]
    internal sealed class SectionCommandHandler :
        ICommandHandler<CreateSectionCommand>,
        ICommandHandler<ChangeSectionCommand>
    {
        public void Handle(ICommandContext context, CreateSectionCommand command)
        {
            context.Add(new Section(command.AggregateRootId, command.Name));
        }
        public void Handle(ICommandContext context, ChangeSectionCommand command)
        {
            context.Get<Section>(command.AggregateRootId).ChangeName(command.Name);
        }
    }
}
