// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// A class with a <see cref="OrderedDictionary{TOrderedDictionary}"/> field that can
    /// be used to draw an 'Add Entry' control below the entries of an actual <see cref="OrderedDictionary{TOrderedDictionary}"/>
    /// in an editor interface (i.e. the inspector).
    /// </summary>
    /// <seealso cref="OrderedDictionary"/>
    /// <typeparam name="TOrderedDictionary">Type of ordered dictionary that is to be edited.</typeparam>
    public abstract class EditableEntry<TOrderedDictionary> : EditableEntry
        where TOrderedDictionary : OrderedDictionary, new()
    {
        [SerializeField]
        private TOrderedDictionary dictionary = new TOrderedDictionary();


        /// <inheritdoc/>
        public override OrderedDictionary Dictionary {
            get { return this.dictionary; }
        }

        /// <inheritdoc/>
        public override object Key {
            get { return this.dictionary.GetKeyFromIndex(0); }
        }

        /// <inheritdoc/>
        public override object Value {
            get { return this.dictionary.GetValueFromIndex(0); }
        }
    }
}
