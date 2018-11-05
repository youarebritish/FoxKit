using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

using UnityEngine;

[CreateAssetMenu(fileName = "New PhysicsFile", menuName = "FoxKit/EntityFile/PhysicsFile", order = 1)]
public class PhysicsFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "ph";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
