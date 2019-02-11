using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

public class LensFlareFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "vfxlf";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
