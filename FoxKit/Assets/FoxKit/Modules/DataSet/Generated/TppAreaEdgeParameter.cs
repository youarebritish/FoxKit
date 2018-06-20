namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using NUnit.Framework;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    [Serializable]
    public class TppAreaEdgeParameter : DataElement
    {
        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private uint fadeTime;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float connectedClearObstruction;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float connectedClearOcclusion;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float connectedBlockedObstruction;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float connectedBlockedOcclusion;

        /// <inheritdoc />
        public override short ClassId => 48;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "fadeTime":
                    this.fadeTime = DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData);
                    break;
                case "connectedClearObstruction":
                    this.connectedClearObstruction = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "connectedClearOcclusion":
                    this.connectedClearOcclusion = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "connectedBlockedObstruction":
                    this.connectedBlockedObstruction = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "connectedBlockedOcclusion":
                    this.connectedBlockedOcclusion = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
            }
        }

    }
}
