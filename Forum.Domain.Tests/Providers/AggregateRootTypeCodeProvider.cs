using ENode.Domain;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.Domain.Tests
{
    public class AggregateRootTypeCodeProvider : AbstractTypeCodeProvider, IAggregateRootTypeCodeProvider
    {
        public AggregateRootTypeCodeProvider()
        {
            RegisterType<Registration>(100);
            RegisterType<Account>(101);
            RegisterType<Section>(102);
            RegisterType<Post>(103);
            RegisterType<Reply>(104);
        }
    }
}
