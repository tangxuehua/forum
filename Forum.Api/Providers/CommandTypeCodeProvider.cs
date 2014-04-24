using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Account;
using Forum.Commands.Post;
using Forum.Commands.Reply;
using Forum.Commands.Section;

namespace Forum.Api.Providers
{
    public class CommandTypeCodeProvider : AbstractTypeCodeProvider, ICommandTypeCodeProvider
    {
        public CommandTypeCodeProvider()
        {
            RegisterType<CreateAccount>(100);

            RegisterType<CreateSection>(200);
            RegisterType<ChangeSectionName>(201);

            RegisterType<CreatePost>(300);
            RegisterType<ChangePostSubjectAndBody>(301);

            RegisterType<CreateReply>(400);
            RegisterType<ChangeReplyBody>(401);
        }
    }
}
