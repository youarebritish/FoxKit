using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;
using FoxKit.Modules.DataSet.Fox.PartsBuilder;

public class PartsFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "parts";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        var entity = new ModelDescription { Name = "ModelDescription0000" };
        return new[] { entity };
    }
}
