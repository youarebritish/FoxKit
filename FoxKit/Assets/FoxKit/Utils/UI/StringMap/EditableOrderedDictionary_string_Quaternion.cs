// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license.

using Rotorz.Games.Collections;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FoxKit.Utils.UI.StringMap
{
    public sealed class EditableOrderedDictionary_string_Quaternion : EditableEntry<OrderedDictionary_string_Quaternion>
    {
    }

    [Serializable, EditableEntry(typeof(EditableOrderedDictionary_string_Quaternion))]
    public sealed class OrderedDictionary_string_Quaternion : OrderedDictionary<string, Quaternion>
    {
    }
}
