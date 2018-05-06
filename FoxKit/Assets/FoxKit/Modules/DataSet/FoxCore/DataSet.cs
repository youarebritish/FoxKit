namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

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
        [SerializeField]
        private DataStringMap dataList = new DataStringMap();

        /// <summary>
        /// The address map.
        /// </summary>
        [SerializeField]
        private AddressEntityDictionary addressMap = new AddressEntityDictionary();
        
        /// <inheritdoc />
        protected override short ClassId => 232;

        /// <summary>
        /// Gets an Entity by name.
        /// </summary>
        /// <param name="key">
        /// The Entity's name.
        /// </param>
        /// <returns>
        /// The <see cref="Entity"/> with the given name.
        /// </returns>
        public Data this[string key] => this.dataList[key];

        /// <summary>
        /// Gets an Entity by address. Only valid for Entities loaded from DataSet files.
        /// </summary>
        /// <param name="address">
        /// The address of the Entity in its DataSet file.
        /// </param>
        /// <returns>
        /// The <see cref="Entity"/> with the given address.
        /// </returns>
        public Entity this[ulong address] => this.addressMap[address];

        /// <summary>
        /// Loads all owned Entities.
        /// </summary>
        public void LoadAllEntities()
        {
            foreach (var data in this.dataList.Values)
            {
                data.OnLoaded();
            }
        }

        /// <summary>
        /// Unloads all owned Entities.
        /// </summary>
        public void UnloadAllEntities()
        {
            foreach (var data in this.dataList.Values)
            {
                data.OnUnloaded();
            }
        }

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            foreach (var data in this.dataList.Values)
            {
                data.OnAssetsImported(tryGetAsset);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Entity> GetChildren()
        {
            return this.GetDataList().Values;
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
        public void AddData(string key, ulong address, Data entity)
        {
            this.dataList.Add(key, entity);
            this.addressMap.Add(address, entity);
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
            return this[key];
        }

        /// <summary>
        /// Get all of the Entities owned by this DataSet.
        /// </summary>
        /// <returns>
        /// All registered <see cref="Data"/> entries with their keys.
        /// </returns>
        public DataStringMap GetDataList()
        {
            return this.dataList;
        }
    }
}