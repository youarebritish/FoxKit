using System;
using System.Collections.Generic;

using FoxKit.Modules.DataSet.FoxCore;

using OdinSerializer;

using UnityEngine;

[CreateAssetMenu(menuName = "FoxKit/DataSet")]
public class DataSetAsset : SerializedScriptableObject
{
    [OdinSerialize, NonSerialized]
    private DataSet DataSet = new DataSet();
    
    public bool IsReadOnly;

    public DataSet GetDataSet()
    {
        return this.DataSet;
    }

    public void SetDataSet(DataSet dataSet)
    {
        this.DataSet = dataSet;
    }
}
