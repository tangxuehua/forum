using System;
using Forum.Infrastructure.Exceptions;

namespace Forum.Infrastructure
{
    public class Assert
    {
        public static void IsNotNull(string name, object obj)
        {
            if (obj == null)
            {
                throw new InvalidArgumentException("{0}不能为空", name);
            }
        }
        public static void IsNotNullOrEmpty(string name, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new InvalidArgumentException("{0}不能为空", name);
            }
        }
        public static void IsNotNullOrWhiteSpace(string name, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new InvalidArgumentException("{0}不能为空", name);
            }
        }
        public static void AreEqual(string id1, string id2, string errorMessageFormat)
        {
            if (id1 != id2)
            {
                throw new InvalidArgumentException(errorMessageFormat, id1, id2);
            }
        }
    }
}
