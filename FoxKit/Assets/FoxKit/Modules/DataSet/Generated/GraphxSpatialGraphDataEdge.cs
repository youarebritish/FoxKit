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
    public class GraphxSpatialGraphDataEdge : DataElement
    {
        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataEdge")]
        private Entity _prevNode;

        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataEdge")]
        private Entity _nextNode;

        /// <inheritdoc />
        public override short ClassId => 36;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("prevNode", Core.PropertyInfoType.EntityHandle, getEntityAddress(this._prevNode)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("nextNode", Core.PropertyInfoType.EntityHandle, getEntityAddress(this._nextNode)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "prevNode":
                    this._prevNode = initFunctions.GetEntityFromAddress(DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData));
                    break;
                case "nextNode":
                    this._nextNode = initFunctions.GetEntityFromAddress(DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData));
                    break;
            }
        }
    }
}
