using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.Des;
using FoxKit.Modules.DataSet.Fox.FoxCore;

using UnityEngine;

[CreateAssetMenu(fileName = "New DestructionFile", menuName = "FoxKit/EntityFile/DestructionFile", order = 1)]
public class DestructionFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "des";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        var entity = new DesParamData { Name = "DesParamData0000" };
        return new[] { entity };
    }
}
