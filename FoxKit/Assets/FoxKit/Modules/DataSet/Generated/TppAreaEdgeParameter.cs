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

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_commFacility_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppAreaEdgeParameter : DataElement
    {
        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private uint _fadeTime;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float _connectedClearObstruction;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float _connectedClearOcclusion;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float _connectedBlockedObstruction;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float _connectedBlockedOcclusion;

        /// <inheritdoc />
        public override short ClassId => 48;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("fadeTime", Core.PropertyInfoType.UInt32, (this._fadeTime)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("connectedClearObstruction", Core.PropertyInfoType.Float, (this._connectedClearObstruction)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("connectedClearOcclusion", Core.PropertyInfoType.Float, (this._connectedClearOcclusion)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("connectedBlockedObstruction", Core.PropertyInfoType.Float, (this._connectedBlockedObstruction)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("connectedBlockedOcclusion", Core.PropertyInfoType.Float, (this._connectedBlockedOcclusion)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "fadeTime":
                    this._fadeTime = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "connectedClearObstruction":
                    this._connectedClearObstruction = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "connectedClearOcclusion":
                    this._connectedClearOcclusion = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "connectedBlockedObstruction":
                    this._connectedBlockedObstruction = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "connectedBlockedOcclusion":
                    this._connectedBlockedOcclusion = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
            }
        }
    }
}
