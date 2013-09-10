using ENode.Commanding;
using ENode.Infrastructure;
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
            context.Add(new Section(command.SectionName));
        }
        public void Handle(ICommandContext context, ChangeSectionName command)
        {
            context.Get<Section>(command.SectionId).ChangeName(command.SectionName);
        }
    }
}
