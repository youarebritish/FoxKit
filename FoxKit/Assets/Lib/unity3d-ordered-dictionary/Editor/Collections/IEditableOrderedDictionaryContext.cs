// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using UnityEditor;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// Interface represents the context of an editable entry.
    /// </summary>
    public interface IEditableOrderedDictionaryContext
    {
        /// <summary>
        /// Gets the unique value that identifies the control.
        /// </summary>
        Guid ControlID { get; }

        /// <summary>
        /// Gets the ordered dictionary that is being edited.
        /// </summary>
        OrderedDictionary OrderedDictionary { get; }

        /// <summary>
        /// Gets the type of the ordered dictionary.
        /// </summary>
        Type OrderedDictionaryType { get; }


        /// <summary>
        /// Creates a <see cref="IOrderedDictionaryListAdaptor"/> that will be used to draw and
        /// manipulate the list of ordered dictionary entries.
        /// </summary>
        /// <param name="dictionaryProperty">Property representing the ordered dictionary.</param>
        /// <returns>
        /// The new <see cref="IOrderedDictionaryListAdaptor"/> instance.
        /// </returns>
        IOrderedDictionaryListAdaptor CreateListAdaptor(SerializedProperty dictionaryProperty);
    }
}
