namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class DataStringMapEditable : EditableEntry<OrderedDictionary_string_Data>
    {
    }

    [Serializable, EditableEntry(typeof(DataStringMapEditable))]
    public sealed class OrderedDictionary_string_Data : OrderedDictionary<string, Data>
    {
    }
}