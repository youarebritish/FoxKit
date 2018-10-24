namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class StringStringMapEditable : EditableEntry<OrderedDictionary_string_string>
    {
    }

    [Serializable, EditableEntry(typeof(StringStringMapEditable))]
    public sealed class OrderedDictionary_string_string : OrderedDictionary<string, string>
    {
    }
}