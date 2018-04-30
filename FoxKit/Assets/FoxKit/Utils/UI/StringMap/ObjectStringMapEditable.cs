namespace FoxKit.Utils.UI.StringMap
{
    using Rotorz.Games.Collections;
    using System;

    public sealed class ObjectStringMapEditable : EditableEntry<ObjectStringMap>
    {
    }

    [Serializable, EditableEntry(typeof(ObjectStringMapEditable))]
    public sealed class ObjectStringMap : OrderedDictionary<string, UnityEngine.Object>
    {
    }
}