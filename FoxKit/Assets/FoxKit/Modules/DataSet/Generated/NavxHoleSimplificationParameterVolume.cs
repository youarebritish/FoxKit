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
    public class NavxHoleSimplificationParameterVolume : TransformData
    {
        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private string _sceneName = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private string _worldName = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private float _convexThreshold;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private float _obbExpandThreshold;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private float _obbToAabbThreshold;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private float _smoothingThreshold;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private bool _isNotClosePassage;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("sceneName", Core.PropertyInfoType.String, (this._sceneName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("worldName", Core.PropertyInfoType.String, (this._worldName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("convexThreshold", Core.PropertyInfoType.Float, (this._convexThreshold)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("obbExpandThreshold", Core.PropertyInfoType.Float, (this._obbExpandThreshold)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("obbToAabbThreshold", Core.PropertyInfoType.Float, (this._obbToAabbThreshold)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("smoothingThreshold", Core.PropertyInfoType.Float, (this._smoothingThreshold)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("isNotClosePassage", Core.PropertyInfoType.Bool, (this._isNotClosePassage)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "sceneName":
                    this._sceneName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "worldName":
                    this._worldName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "convexThreshold":
                    this._convexThreshold = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "obbExpandThreshold":
                    this._obbExpandThreshold = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "obbToAabbThreshold":
                    this._obbToAabbThreshold = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "smoothingThreshold":
                    this._smoothingThreshold = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "isNotClosePassage":
                    this._isNotClosePassage = (DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData));
                    break;
            }
        }
    }
}
