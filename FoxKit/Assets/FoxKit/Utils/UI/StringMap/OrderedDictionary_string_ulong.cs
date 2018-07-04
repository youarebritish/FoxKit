namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class UlongStringMapEditable : EditableEntry<OrderedDictionary_string_ulong>
    {
    }

    [Serializable, EditableEntry(typeof(UlongStringMapEditable))]
    public sealed class OrderedDictionary_string_ulong : OrderedDictionary<ulong, string>
    {
    }
}