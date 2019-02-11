using System;
using System.Collections.Generic;

using FoxKit.Modules.DataSet.Fox.FoxCore;

using OdinSerializer;

public abstract class EntityFileAsset : SerializedScriptableObject
{
    [OdinSerialize, NonSerialized]
    private DataSet DataSet = new DataSet();
    
    public bool IsReadOnly;

    public string PackageGuid;

    /// <summary>
    /// Gets the file extension to use when exporting.
    /// </summary>
    public abstract string Extension { get; }

    public DataSet GetDataSet()
    {
        return this.DataSet;
    }

    public void SetDataSet(DataSet dataSet)
    {
        this.DataSet = dataSet;
    }

    public void Initialize()
    {
        foreach (var entity in this.MakeInitialEntities())
        {
            this.DataSet.AddData(entity.Name, entity);
        }
    }

    /// <summary>
    /// Create Entities to auto-populate new instances of this asset.
    /// </summary>
    /// <returns>
    /// The Entities to add when a new instance is created.
    /// </returns>
    protected abstract IEnumerable<Data> MakeInitialEntities();
}
