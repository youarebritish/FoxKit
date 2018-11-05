using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.Sdx;

using UnityEngine;

[CreateAssetMenu(fileName = "New SoundFile", menuName = "FoxKit/EntityFile/SoundFile", order = 1)]
public class SoundFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "phsd";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        var entity = new SoundPhysicsParameter { Name = "SoundPhysicsParameter0000" };
        return new[] { entity };
    }
}
