using System;

namespace test.Extensions
{
    public static class EnumExtension
    {
        public static T GetEnumByName<T>(string name)
        {
            return (T)System.Enum.Parse(typeof(T), name);
        }
    }
}
