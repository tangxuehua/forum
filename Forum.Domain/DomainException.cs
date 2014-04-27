using System;

namespace Forum.Domain
{
    public class DomainException : Exception
    {
        public DomainException(string message, params object[] args) : base(string.Format(message, args)) { }
    }
}
