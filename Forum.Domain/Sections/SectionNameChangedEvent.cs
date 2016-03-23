﻿using System;
using ENode.Eventing;
using ENode.Infrastructure;

namespace Forum.Domain.Sections
{
    public class SectionNameChangedEvent : DomainEvent<string>
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        private SectionNameChangedEvent() { }
        public SectionNameChangedEvent(string name,string description)
        {
            Name = name;
            Description = description;
        }
    }
}
