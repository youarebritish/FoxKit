// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license.

using System;
using UnityEngine;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// An object that allows users to edit <see cref="OrderedDictionary_string_Sprite"/>
    /// objects when using the Unity inspector.
    /// </summary>
    public sealed class EditableOrderedDictionary_string_Sprite : EditableEntry<OrderedDictionary_string_Sprite>
    {
    }


    /// <summary>
    /// An ordered dictionary with <see langword="string"/> keys mapped to
    /// <see cref="Sprite"/> values.
    /// </summary>
    [Serializable, EditableEntry(typeof(EditableOrderedDictionary_string_Sprite))]
    public sealed class OrderedDictionary_string_Sprite : OrderedDictionary<string, Sprite>
    {
    }
}
