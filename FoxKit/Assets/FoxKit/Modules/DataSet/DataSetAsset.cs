using System;

using FoxKit.Modules.Archive;
using FoxKit.Modules.DataSet.FoxCore;

using OdinSerializer;

using UnityEngine;

[CreateAssetMenu(fileName = "New DataSet", menuName = "FoxKit/DataSet", order = 1)]
public class DataSetAsset : SerializedScriptableObject
{
    [OdinSerialize, NonSerialized]
    private DataSet DataSet = new DataSet();

    [OdinSerialize, NonSerialized]
    private PackageDefinition package;
    
    public bool IsReadOnly;

    public PackageDefinition Package
    {
        get
        {
            return this.package;
        }
        set
        {
            this.package = value;
        }
    }

    public DataSet GetDataSet()
    {
        return this.DataSet;
    }

    public void SetDataSet(DataSet dataSet)
    {
        this.DataSet = dataSet;
    }
}
