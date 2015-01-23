using ECommon.Components;
using ENode.Domain;
using ENode.Infrastructure.Impl;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;

namespace Forum.Domain.Tests
{
    [Component]
    public class AggregateRootTypeCodeProvider : DefaultTypeCodeProvider<IAggregateRoot>
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
