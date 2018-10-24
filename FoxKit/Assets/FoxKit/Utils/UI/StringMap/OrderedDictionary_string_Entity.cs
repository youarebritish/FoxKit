namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class EntityStringMapEditable : EditableEntry<OrderedDictionary_string_Entity>
    {
    }

    [Serializable, EditableEntry(typeof(EntityStringMapEditable))]
    public sealed class OrderedDictionary_string_Entity : OrderedDictionary<string, Entity>
    {
    }
}