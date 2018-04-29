namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class UlongStringMapEditable : EditableEntry<UlongStringMap>
    {
    }

    [Serializable, EditableEntry(typeof(UlongStringMapEditable))]
    public sealed class UlongStringMap : OrderedDictionary<ulong, string>
    {
    }
}