using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.Anim;
using FoxKit.Modules.DataSet.Fox.FoxCore;

using UnityEngine;

[CreateAssetMenu(fileName = "New FacialSettingFile", menuName = "FoxKit/EntityFile/FacialSettingFile", order = 1)]
public class FacialSettingFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "fsd";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        var entity = new FacialSettingData { Name = "FacialSettingData0000" };
        return new[] { entity };
    }
}
