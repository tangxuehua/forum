using ECommon.Components;
using ENode.Commanding;
using Forum.Commands.Sections;
using Forum.Domain.Sections;

namespace Forum.CommandHandlers
{
    public class SectionCommandHandler :
        ICommandHandler<CreateSectionCommand>,
        ICommandHandler<ChangeSectionNameCommand>
    {
        public void Handle(ICommandContext context, CreateSectionCommand command)
        {
            context.Add(new Section(command.AggregateRootId, command.Name));
        }
        public void Handle(ICommandContext context, ChangeSectionNameCommand command)
        {
            context.Get<Section>(command.AggregateRootId).ChangeNameAndDescription(command.Name, command.Description);
        }
    }
}
