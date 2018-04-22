// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// Associates a type of <see cref="EditableEntry"/> with a type of <see cref="OrderedDictionary"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class EditableEntryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableEntryAttribute"/> class.
        /// </summary>
        /// <param name="editableEntryType">The type of editable entry.</param>
        public EditableEntryAttribute(Type editableEntryType)
        {
            this.EditableEntryType = editableEntryType;
        }


        /// <summary>
        /// Gets the type of <see cref="EditableEntry"/> that is to be associated with
        /// the <see cref="OrderedDictionary"/> class.
        /// </summary>
        public Type EditableEntryType { get; private set; }
    }
}
