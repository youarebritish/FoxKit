using System;

using FoxKit.Modules.DataSet.FoxCore;

using OdinSerializer;

using UnityEngine;

[CreateAssetMenu(fileName = "New DataSet", menuName = "FoxKit/DataSet", order = 1)]
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
