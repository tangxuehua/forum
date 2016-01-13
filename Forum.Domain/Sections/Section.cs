using System;
using ENode.Domain;
using ENode.Infrastructure;
using Forum.Infrastructure;

namespace Forum.Domain.Sections
{
    public class Section : AggregateRoot<string>
    {
        private string _name;

        public Section(string id, string name)
            : base(id)
        {
            Assert.IsNotNullOrWhiteSpace("版块名称", name);
            if (name.Length > 128)
            {
                throw new Exception("版块名称长度不能超过128");
            }
            ApplyEvent(new SectionCreatedEvent(name));
        }

        public void ChangeName(string name)
        {
            ApplyEvent(new SectionNameChangedEvent( name));
        }

        private void Handle(SectionCreatedEvent evnt)
        {
            _name = evnt.Name;
        }
        private void Handle(SectionNameChangedEvent evnt)
        {
            _name = evnt.Name;
        }
    }
}