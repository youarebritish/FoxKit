namespace FoxKit.Utils
{
    using System.Collections.Generic;

    /// <summary>
    /// Helper functions for working with Collections.
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// Populates a collection with instances of a specific value.
        /// </summary>
        /// <typeparam name="T">The collection type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="value">The value to insert.</param>
        /// <param name="times">Number of times to insert the value.</param>
        public static void Populate<T>(this ICollection<T> collection, T value, uint times)
        {
            for (var i = 0; i < times; i++)
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// Populates a collection with default values.
        /// </summary>
        /// <typeparam name="T">The collection type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="times">Number of times to insert the collection's default value.</param>
        public static void Populate<T>(this ICollection<T> collection, uint times)
        {
            Populate(collection, default(T), times);
        }
    }
}