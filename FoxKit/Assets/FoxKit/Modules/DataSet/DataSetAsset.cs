using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.FoxGameKit;

public class DataSetAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "fox2";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        var texturePackLoadConditioner = new TexturePackLoadConditioner { Name = "TexturePackLoadConditioner0000" };
        return new[] { texturePackLoadConditioner };
    }
}
