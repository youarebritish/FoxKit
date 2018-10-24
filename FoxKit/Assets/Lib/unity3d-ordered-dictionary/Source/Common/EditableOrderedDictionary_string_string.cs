// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// An object that allows users to edit <see cref="OrderedDictionary_string_string"/>
    /// objects when using the Unity inspector.
    /// </summary>
    public sealed class EditableOrderedDictionary_string_string : EditableEntry<OrderedDictionary_string_string>
    {
    }


    /// <summary>
    /// An ordered dictionary with <see langword="string"/> keys mapped to other
    /// <see langword="string"/ values.
    /// </summary>
    [Serializable, EditableEntry(typeof(EditableOrderedDictionary_string_string))]
    public sealed class OrderedDictionary_string_string : OrderedDictionary<string, string>
    {
    }
}
