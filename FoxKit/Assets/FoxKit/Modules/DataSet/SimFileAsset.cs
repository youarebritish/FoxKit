using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

public class SimFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "sim";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
