using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

public class PhysicsFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "ph";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
