// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEditor;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// Interface represents the context of an editable entry.
    /// </summary>
    public interface IOrderedDictionaryListAdaptor : IReorderableListAdaptor
    {
        /// <summary>
        /// Gets a value indicating whether a null key error was encountered.
        /// </summary>
        bool HadNullKeyErrorOnLastRepaint { get; }


        /// <summary>
        /// Adds a new entry to the ordered dictionary.
        /// </summary>
        /// <param name="inputKeyProperty">Key for the new entry (will be copied from
        /// another serialized object).</param>
        /// <param name="inputValueProperty">Value for the new entry (will be copied from
        /// another serialized object).</param>
        /// <exception cref="System.InvalidOperationException">
        /// If entry cannot be added because the collection has an inconsistent quantity
        /// of keys and values.
        /// </exception>
        void Add(SerializedProperty inputKeyProperty, SerializedProperty inputValueProperty);
    }
}
