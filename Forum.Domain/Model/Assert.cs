using System;

namespace Forum.Domain.Model
{
    internal class Assert
    {
        public static void IsNotNull(string name, object obj)
        {
            if (obj == null)
            {
                throw new Exception(string.Format("{0}不能为空", name));
            }
        }
        public static void IsNotNullOrEmpty(string name, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new Exception(string.Format("{0}不能为空或空字符串", name));
            }
        }
        public static void IsNotNullOrWhiteSpace(string name, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception(string.Format("{0}不能为空或空白字符串", name));
            }
        }
    }
}
