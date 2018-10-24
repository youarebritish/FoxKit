// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license.

using System;
using UnityEngine;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// An object that allows users to edit <see cref="OrderedDictionary_string_Texture"/>
    /// objects when using the Unity inspector.
    /// </summary>
    public sealed class EditableOrderedDictionary_string_Texture : EditableEntry<OrderedDictionary_string_Texture>
    {
    }


    /// <summary>
    /// An ordered dictionary with <see langword="string"/> keys mapped to
    /// <see cref="Texture"/> values.
    /// </summary>
    [Serializable, EditableEntry(typeof(EditableOrderedDictionary_string_Texture))]
    public sealed class OrderedDictionary_string_Texture : OrderedDictionary<string, Texture>
    {
    }
}
