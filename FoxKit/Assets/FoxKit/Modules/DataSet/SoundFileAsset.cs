using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.Sdx;

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
