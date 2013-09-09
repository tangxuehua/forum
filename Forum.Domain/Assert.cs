using System;

namespace Forum.Domain
{
    internal class Assert
    {
        public static void IsNotNull(string name, object obj)
        {
            if (obj == null)
            {
                throw new DomainException("{0}不能为空", name);
            }
        }
        public static void IsNotNullOrEmpty(string name, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new DomainException("{0}不能为空或空字符串", name);
            }
        }
        public static void IsNotNullOrWhiteSpace(string name, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new DomainException("{0}不能为空或空白字符串", name);
            }
        }
        public static void AreEqual(Guid id1, Guid id2, string errorMessageFormat)
        {
            if (id1 != id2)
            {
                throw new DomainException(errorMessageFormat, id1, id2);
            }
        }
    }
}
