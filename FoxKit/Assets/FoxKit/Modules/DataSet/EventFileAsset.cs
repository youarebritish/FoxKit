using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

using UnityEngine;

[CreateAssetMenu(fileName = "New EventFile", menuName = "FoxKit/EntityFile/EventFile", order = 1)]
public class EventFileAsset : EntityFileAsset
{
    /// <inheritdoc />
    public override string Extension => "evf";

    protected override IEnumerable<Data> MakeInitialEntities()
    {
        return new List<Data>();
    }
}
