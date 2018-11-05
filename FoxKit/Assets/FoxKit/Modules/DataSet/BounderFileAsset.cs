using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.Navx;

using UnityEngine;

[CreateAssetMenu(fileName = "New BounderFile", menuName = "FoxKit/EntityFile/BounderFile", order = 1)]
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
