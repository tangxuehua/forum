using System;
using ENode.Domain;
using ENode.Infrastructure;
using Forum.Infrastructure;

namespace Forum.Domain.Sections
{
    public class Section : AggregateRoot<string>
    {
        private string _name;
        private string _description;
        public Section(string id, string name, string description)
            : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("版块名称", name);
            Assert.IsNotNullOrWhiteSpace("版块描述", description);
            if (name.Length > 128)
            {
                throw new Exception("版块名称长度不能超过128");
            }
            ApplyEvent(new SectionCreatedEvent(name, description));
        }
      
        public void ChangeNameAndDescription(string name, string description)
        {
            ApplyEvent(new SectionChangedEvent(name, description));
        }
        private void Handle(SectionChangedEvent evnt)
        {
            _name = evnt.Name;
            _description = evnt.Description;
        }
        private void Handle(SectionCreatedEvent evnt)
        {
            _name = evnt.Name;
        }
    }
}