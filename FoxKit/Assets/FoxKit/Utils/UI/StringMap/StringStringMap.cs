namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class StringStringMapEditable : EditableEntry<StringStringMap>
    {
    }

    [Serializable, EditableEntry(typeof(StringStringMapEditable))]
    public sealed class StringStringMap : OrderedDictionary<string, string>
    {
    }
}