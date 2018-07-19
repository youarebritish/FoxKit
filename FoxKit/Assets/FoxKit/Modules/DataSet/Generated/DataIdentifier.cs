namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using NUnit.Framework;

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file gntn_common_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class DataIdentifier : Data
    {
        [SerializeField, Modules.DataSet.Property("DataIdentifier")]
        private string _identifier = string.Empty;

        [SerializeField, Modules.DataSet.Property("DataIdentifier")]
        private OrderedDictionary_string_EntityLink _links;

        /// <inheritdoc />
        public override short ClassId => 168;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("identifier", Core.PropertyInfoType.String, (this._identifier)));
            parentProperties.Add(PropertyInfoFactory.MakeStringMapProperty("links", Core.PropertyInfoType.EntityLink, this._links.ToDictionary(entry => entry.Key, entry => convertEntityLink(entry.Value) as object)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "identifier":
                    this._identifier = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "links":
                    var linksDictionary = DataSetUtils.GetStringMap<Core.EntityLink>(propertyData);
                    var linksFinalValues = new OrderedDictionary_string_EntityLink();
                    
                    foreach(var entry in linksDictionary)
                    {
                        linksFinalValues.Add(entry.Key, initFunctions.MakeEntityLink(entry.Value));
                    }
                    
                    this._links = linksFinalValues;
                    break;
            }
        }
    }
}
