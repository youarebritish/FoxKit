using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.Sdx;

using UnityEngine;

[CreateAssetMenu(fileName = "New SimFile", menuName = "FoxKit/EntityFile/SimFile", order = 1)]
public class SimFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "sim";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
