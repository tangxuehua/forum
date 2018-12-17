using System.Threading.Tasks;
using ENode.Commanding;
using Forum.Commands.Sections;
using Forum.Domain.Sections;

namespace Forum.CommandHandlers
{
    public class SectionCommandHandler :
        ICommandHandler<CreateSectionCommand>,
        ICommandHandler<ChangeSectionCommand>
    {
        public Task HandleAsync(ICommandContext context, CreateSectionCommand command)
        {
            return context.AddAsync(new Section(command.AggregateRootId, command.Name, command.Description));
        }
        public async Task HandleAsync(ICommandContext context, ChangeSectionCommand command)
        {
            var section = await context.GetAsync<Section>(command.AggregateRootId);
            section.ChangeSection(command.Name, command.Description);
        }
    }
}
