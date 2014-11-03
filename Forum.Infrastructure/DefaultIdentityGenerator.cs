using ECommon.Components;
using ECommon.Utilities;

namespace Forum.Infrastructure
{
    [Component]
    public class DefaultIdentityGenerator : IIdentityGenerator
    {
        public string GetNextIdentity()
        {
            return ObjectId.GenerateNewStringId();
        }
    }
}
