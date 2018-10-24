using System;

namespace FoxKit.Utils
{
    public static class MathUtils
    {
        public static T Clamp<T>(T value, T min, T max)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            return value.CompareTo(max) > 0 ? max : value;
        }
    }
}