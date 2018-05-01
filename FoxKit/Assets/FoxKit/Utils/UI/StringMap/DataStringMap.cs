namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class DataStringMapEditable : EditableEntry<DataStringMap>
    {
    }

    [Serializable, EditableEntry(typeof(DataStringMapEditable))]
    public sealed class DataStringMap : OrderedDictionary<string, Data>
    {
    }
}