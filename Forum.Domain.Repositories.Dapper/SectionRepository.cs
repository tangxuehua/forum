using ECommon.Components;
using ECommon.Utilities;
using Forum.Domain.Sections;

namespace Forum.Domain.Repositories.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class SectionRepository : ISectionRepository
    {
        public string GetNextSectionId()
        {
            return ObjectId.GenerateNewStringId();
        }
    }
}
