using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.Navx;

public class BounderFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "bnd";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        var entity = new NavxSquareGraphBounderData { Name = "NavxSquareGraphBounderData0000" };
        return new[] { entity };
    }
}
