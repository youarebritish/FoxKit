namespace FoxKit.Utils
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Performs structural equality comparison.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StructuralEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if x and y are equal.</returns>
        public bool Equals(T x, T y)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
        }
    }
}