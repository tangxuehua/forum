using System;

namespace Forum.Infrastructure.Exceptions
{
    public class InvalidRegistrationStatusException : Exception
    {
        public InvalidRegistrationStatusException(string message, params object[] args) : base(string.Format(message, args)) { }
    }
}
