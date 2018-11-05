using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.Sim;

public class ClothSettingFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "clo";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        var entity = new SimClothSettingData { Name = "SimClothSettingData0000" };
        return new[] { entity };
    }
}
