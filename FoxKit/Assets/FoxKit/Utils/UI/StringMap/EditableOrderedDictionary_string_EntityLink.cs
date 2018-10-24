// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license.

using Rotorz.Games.Collections;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;

    public sealed class EditableOrderedDictionary_string_EntityLink : EditableEntry<OrderedDictionary_string_EntityLink>
    {
    }

    [Serializable, EditableEntry(typeof(EditableOrderedDictionary_string_EntityLink))]
    public sealed class OrderedDictionary_string_EntityLink : OrderedDictionary<string, EntityLink>
    {
    }
}
