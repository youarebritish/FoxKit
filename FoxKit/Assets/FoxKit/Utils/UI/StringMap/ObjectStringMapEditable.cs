namespace FoxKit.Utils.UI.StringMap
{
    using Rotorz.Games.Collections;
    using System;

    public sealed class ObjectStringMapEditable : EditableEntry<OrderedDictionary_string_Object>
    {
    }

    [Serializable, EditableEntry(typeof(ObjectStringMapEditable))]
    public sealed class OrderedDictionary_string_Object : OrderedDictionary<string, UnityEngine.Object>
    {
    }
}