using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

using UnityEngine;

[CreateAssetMenu(fileName = "New VehicleFile", menuName = "FoxKit/EntityFile/VehicleFile", order = 1)]
public class VehicleFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "veh";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
