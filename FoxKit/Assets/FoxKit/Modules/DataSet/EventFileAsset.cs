using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

public class EventFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "evf";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
