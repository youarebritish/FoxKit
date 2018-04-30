namespace FoxKit.Utils
{
    using FoxKit.Modules.DataSet.FoxCore;
    using Rotorz.Games.Collections;
    using System;

    public sealed class AddressEntityDictionaryEditable : EditableEntry<AddressEntityDictionary>
    {
    }

    [Serializable, EditableEntry(typeof(AddressEntityDictionaryEditable))]
    public sealed class AddressEntityDictionary : OrderedDictionary<ulong, Entity>
    {
    }
}