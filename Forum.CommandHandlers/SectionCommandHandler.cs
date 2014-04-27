using ECommon.Components;
using ENode.Commanding;
using Forum.Commands.Sections;
using Forum.Domain.Sections;

namespace Forum.CommandHandlers
{
    [Component(LifeStyle.Singleton)]
    public class SectionCommandHandler :
        ICommandHandler<CreateSectionCommand>,
        ICommandHandler<UpdateSectionCommand>
    {
        public void Handle(ICommandContext context, CreateSectionCommand command)
        {
            context.Add(new Section(command.AggregateRootId, command.Name));
        }
        public void Handle(ICommandContext context, UpdateSectionCommand command)
        {
            context.Get<Section>(command.AggregateRootId).Update(command.Name);
        }
    }
}
