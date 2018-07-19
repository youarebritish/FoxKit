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
    public class NavxAttributePathVolume : TransformData
    {
        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private List<Entity> _nodes;

        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private List<Entity> _edges;

        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private UnityEngine.Vector3 _topPos;

        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private string _worldName = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private List<Entity> _attributeInfos;

        /// <inheritdoc />
        public override short ClassId => 336;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("nodes", Core.PropertyInfoType.EntityPtr, (from propertyEntry in this._nodes select getEntityAddress(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("edges", Core.PropertyInfoType.EntityPtr, (from propertyEntry in this._edges select getEntityAddress(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("topPos", Core.PropertyInfoType.Vector3, FoxUtils.UnityToFox(this._topPos)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("worldName", Core.PropertyInfoType.String, (this._worldName)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("attributeInfos", Core.PropertyInfoType.EntityPtr, (from propertyEntry in this._attributeInfos select getEntityAddress(propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "nodes":
                    this._nodes = (from rawValue in DataSetUtils.GetDynamicArrayValues<ulong>(propertyData) select initFunctions.GetEntityFromAddress(rawValue)).ToList();
                    break;
                case "edges":
                    this._edges = (from rawValue in DataSetUtils.GetDynamicArrayValues<ulong>(propertyData) select initFunctions.GetEntityFromAddress(rawValue)).ToList();
                    break;
                case "topPos":
                    this._topPos = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Vector3>(propertyData));
                    break;
                case "worldName":
                    this._worldName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "attributeInfos":
                    this._attributeInfos = (from rawValue in DataSetUtils.GetDynamicArrayValues<ulong>(propertyData) select initFunctions.GetEntityFromAddress(rawValue)).ToList();
                    break;
            }
        }
    }
}
