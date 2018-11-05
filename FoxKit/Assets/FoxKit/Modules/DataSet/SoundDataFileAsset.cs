using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.Sdx;

using UnityEngine;

[CreateAssetMenu(fileName = "New SoundDataFile", menuName = "FoxKit/EntityFile/SoundDataFile", order = 1)]
public class SoundDataFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "sdf";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        var entity = new SoundDataFileInfo { Name = "SoundDataFileInfo0000" };
        return new[] { entity };
    }
}
