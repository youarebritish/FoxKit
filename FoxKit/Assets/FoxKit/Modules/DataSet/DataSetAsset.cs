using System;

using FoxKit.Modules.DataSet.FoxCore;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class DataSetAsset : SerializedScriptableObject
{
    [OdinSerialize, NonSerialized]
    public DataSet DataSet = new DataSet();
}
