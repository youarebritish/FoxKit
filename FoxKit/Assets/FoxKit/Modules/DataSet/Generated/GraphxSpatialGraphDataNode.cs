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

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class GraphxSpatialGraphDataNode : DataElement
    {
        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataNode")]
        private UnityEngine.Vector3 _position;

        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataNode")]
        private List<Entity> _inlinks;

        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataNode")]
        private List<Entity> _outlinks;

        /// <inheritdoc />
        public override short ClassId => 80;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("position", Core.PropertyInfoType.Vector3, FoxUtils.UnityToFox(this._position)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("inlinks", Core.PropertyInfoType.EntityHandle, (from propertyEntry in this._inlinks select getEntityAddress(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("outlinks", Core.PropertyInfoType.EntityHandle, (from propertyEntry in this._outlinks select getEntityAddress(propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "position":
                    this._position = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Vector3>(propertyData));
                    break;
                case "inlinks":
                    this._inlinks = (from rawValue in DataSetUtils.GetDynamicArrayValues<ulong>(propertyData) select initFunctions.GetEntityFromAddress(rawValue)).ToList();
                    break;
                case "outlinks":
                    this._outlinks = (from rawValue in DataSetUtils.GetDynamicArrayValues<ulong>(propertyData) select initFunctions.GetEntityFromAddress(rawValue)).ToList();
                    break;
            }
        }
    }
}
