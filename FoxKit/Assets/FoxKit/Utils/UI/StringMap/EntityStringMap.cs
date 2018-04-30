namespace FoxKit.Utils.UI.StringMap
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class EntityStringMapEditable : EditableEntry<EntityStringMap>
    {
    }

    [Serializable, EditableEntry(typeof(EntityStringMapEditable))]
    public sealed class EntityStringMap : OrderedDictionary<string, Entity>
    {
    }
}