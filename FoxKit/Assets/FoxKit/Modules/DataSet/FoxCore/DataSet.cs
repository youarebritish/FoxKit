namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.Exporter;

    using FoxLib;
    
    using Sirenix.Serialization;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Container for a set of Fox Engine entities.
    /// </summary>
    [Serializable]
    public class DataSet : Data
    {
        /// <summary>
        /// The data list.
        /// </summary>
        [OdinSerialize]
        public Dictionary<string, Data> dataList = new Dictionary<string, Data>();
        
        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(BoxCollider)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 232;
        
        /// <summary>
        /// Loads all owned Entities.
        /// </summary>
        public void LoadAllEntities()
        {
            foreach (var data in this.dataList.Values)
            {
                data.OnLoaded();
            }

            foreach (var data in this.dataList.Values)
            {
                data.PostOnLoaded();
            }
        }

        /// <summary>
        /// Unloads all owned Entities.
        /// </summary>
        public void UnloadAllEntities()
        {
            foreach (var data in this.dataList.Values)
            {
                data?.OnUnloaded();
            }
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStringMapProperty(
                "dataList",
                Core.PropertyInfoType.EntityPtr,
                this.dataList.ToDictionary(entry => entry.Key, entry => getEntityAddress(entry.Value) as object)));
            return parentProperties;
        }

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            foreach (var kvp in this.dataList)
            {
                kvp.Value.OnAssetsImported(tryGetAsset);
            }
        }

        /// <summary>
        /// Adds an Entity to the DataSet.
        /// </summary>
        /// <param name="key">
        /// The string key (name) of the Entity.
        /// </param>
        /// <param name="address">
        /// The memory address of the Entity. Only needed when loading from a DataSet file.
        /// </param>
        /// <param name="entity">
        /// The entity to add.
        /// </param>
        public void AddData(string key, Data entity)
        {
            if (entity != null)
            {
                this.dataList.Add(key, entity);
            }
        }
        
        /// <summary>
        /// Removes an Entity with the given key.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public void RemoveData(string key)
        {
            // TODO
        }
        
        /// <summary>
        /// Get an entry by name.
        /// </summary>
        /// <param name="key">
        /// The string key (name) of the Entity.
        /// </param>
        /// <returns>
        /// The <see cref="Entity"/> with the given key.
        /// </returns>
        public Entity GetData(string key)
        {
            return this.dataList[key];
        }

        /// <summary>
        /// Get all of the Entities owned by this DataSet.
        /// </summary>
        /// <returns>
        /// All registered <see cref="Data"/> entries with their keys.
        /// </returns>
        public Dictionary<string, Data> GetDataList()
        {
            return this.dataList;
        }
    }
}