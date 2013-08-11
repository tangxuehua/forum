using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Application.Commands;
using Forum.Domain.Model;

namespace Forum.Application.CommandHandlers
{
    [Component]
    public class SectionCommandHandler :
        ICommandHandler<CreateSection>,
        ICommandHandler<ChangeSectionName>
    {
        public void Handle(ICommandContext context, CreateSection command)
        {
            context.Add(new Section(command.SectionName));
        }
        public void Handle(ICommandContext context, ChangeSectionName command)
        {
            context.Get<Section>(command.SectionId).ChangeName(command.SectionName);
        }
    }
}
