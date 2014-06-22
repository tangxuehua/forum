using System;

namespace Forum.Infrastructure.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string message, params object[] args) : base(string.Format(message, args)) { }
    }
}
