using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

using UnityEngine;

[CreateAssetMenu(fileName = "New LensFlareFile", menuName = "FoxKit/EntityFile/LensFlareFile", order = 1)]
public class LensFlareFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "vfxlf";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
