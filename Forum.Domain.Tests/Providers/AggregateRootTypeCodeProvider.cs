using ENode.Domain;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.Domain.Tests
{
    public class AggregateRootTypeCodeProvider : AbstractTypeCodeProvider<IAggregateRoot>, ITypeCodeProvider<IAggregateRoot>
    {
        public AggregateRootTypeCodeProvider()
        {
            RegisterType<Account>(100);
            RegisterType<Section>(101);
            RegisterType<Post>(102);
            RegisterType<Reply>(103);
        }
    }
}
