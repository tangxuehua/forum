using ECommon.Components;

namespace Forum.Domain.Sections
{
    [Component(LifeStyle.Singleton)]
    public class SectionFactory
    {
        private ISectionRepository _repository;

        public SectionFactory(ISectionRepository repository)
        {
            _repository = repository;
        }

        public Section CreateSection(string name)
        {
            return new Section(_repository.GetNextSectionId(), name);
        }
    }
}
