namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

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
        [SerializeField, HideInInspector]
        private OrderedDictionary_string_Data orderedDictionaryStringDataList = new OrderedDictionary_string_Data();
        
        /// <summary>
        /// The address map.
        /// </summary>
        [SerializeField, HideInInspector]
        private AddressEntityDictionary addressMap = new AddressEntityDictionary();

        /// <inheritdoc />
        public override IEnumerable<Entity> Children => this.GetDataList().Values;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(BoxCollider)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 232;

        /// <inheritdoc />
        public override string Name => string.Empty;

        /// <summary>
        /// Gets an Entity by name.
        /// </summary>
        /// <param name="key">
        /// The Entity's name.
        /// </param>
        /// <returns>
        /// The <see cref="Entity"/> with the given name.
        /// </returns>
        public Data this[string key] => this.orderedDictionaryStringDataList[key];

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
            foreach (var data in this.orderedDictionaryStringDataList.Values)
            {
                data.OnLoaded();
            }

            foreach (var data in this.orderedDictionaryStringDataList.Values)
            {
                data.PostOnLoaded();
            }
        }

        /// <summary>
        /// Unloads all owned Entities.
        /// </summary>
        public void UnloadAllEntities()
        {
            foreach (var data in this.orderedDictionaryStringDataList.Values)
            {
                if (data == null)
                {
                    continue;
                }

                data.OnUnloaded();
            }
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStringMapProperty(
                "orderedDictionaryStringDataList",
                Core.PropertyInfoType.EntityPtr,
                this.orderedDictionaryStringDataList.ToDictionary(entry => entry.Key, entry => getEntityAddress(entry.Value) as object)));
            return parentProperties;
        }

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            foreach (var data in this.orderedDictionaryStringDataList.Values)
            {
                data.OnAssetsImported(tryGetAsset);
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
        public void AddData(string key, ulong address, Data entity)
        {
            this.orderedDictionaryStringDataList.Add(key, entity);
            this.addressMap.Add(address, entity);
        }

        /// <summary>
        /// Adds an Entity to the DataSet without assigning it an address.
        /// </summary>
        /// <param name="key">
        /// The string key (name) of the Entity.
        /// </param>
        /// <param name="entity">
        /// The entity to add.
        /// </param>
        public void AddData(string key, Data entity)
        {
            this.orderedDictionaryStringDataList.Add(key, entity);

            var highestAddress = this.addressMap.Keys.Max(address => address);
            this.addressMap.Add(highestAddress + 1, entity);
        }

        /// <summary>
        /// Removes an Entity with the given key.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public void RemoveData(string key)
        {
            var entity = this.orderedDictionaryStringDataList[key];
            this.orderedDictionaryStringDataList.Remove(key);

            ulong address = 0;
            var foundAddress = false;
            foreach (var entry in this.addressMap)
            {
                if (entry.Value != entity)
                {
                    continue;
                }

                foundAddress = true;
                address = entry.Key;
                break;
            }

            if (foundAddress)
            {
                this.addressMap.Remove(address);
            }
        }

        /// <summary>
        /// Registers a DataElement to the DataSet.
        /// </summary>
        /// <param name="address">
        /// The memory address of the DataElement. Only needed when loading from a DataSet file.
        /// </param>
        /// <param name="dataElement">
        /// The DataElement to add.
        /// </param>
        public void AddDataElement(ulong address, Entity dataElement)
        {
            this.addressMap.Add(address, dataElement);
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
        public OrderedDictionary_string_Data GetDataList()
        {
            return this.orderedDictionaryStringDataList;
        }

        public IEnumerable<Entity> GetAllEntities()
        {
            return this.addressMap.Values;
        }
    }
}