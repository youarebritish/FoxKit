using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

public class VehicleFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "veh";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
