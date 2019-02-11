using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.Sdx;

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
