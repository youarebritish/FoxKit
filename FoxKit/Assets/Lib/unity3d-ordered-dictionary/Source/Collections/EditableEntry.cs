// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// A class with a <see cref="OrderedDictionary"/> field that can be used to draw
    /// an 'Add Entry' control below the entries of an actual <see cref="OrderedDictionary"/>
    /// in an editor interface (i.e. the inspector).
    /// </summary>
    /// <seealso cref="OrderedDictionary"/>
    public abstract class EditableEntry : ScriptableObject
    {
        /// <summary>
        /// Gets the <see cref="OrderedDictionary"/> that has exactly one key/value entry.
        /// </summary>
        /// <remarks>
        /// <para>This property is exposed so that a list adaptor can be constructed to
        /// draw the 'Add Entry' controls.</para>
        /// </remarks>
        public abstract OrderedDictionary Dictionary { get; }

        /// <summary>
        /// Gets the user input for the new entry key.
        /// </summary>
        public abstract object Key { get; }

        /// <summary>
        /// Gets the user input for the new entry value.
        /// </summary>
        public abstract object Value { get; }
    }
}
