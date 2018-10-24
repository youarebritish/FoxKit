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

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class GeoModuleCondition : TransformData
    {
        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private string _trapCategory = string.Empty;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private uint _trapPriority;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private bool _enable;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private bool _isOnce;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private bool _isAndCheck;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private List<string> _checkFuncNames;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private List<string> _execFuncNames;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private List<Entity> _checkCallbackDataElements;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private List<Entity> _execCallbackDataElements;

        /// <inheritdoc />
        public override short ClassId => 352;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("trapCategory", Core.PropertyInfoType.String, (this._trapCategory)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("trapPriority", Core.PropertyInfoType.UInt32, (this._trapPriority)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("enable", Core.PropertyInfoType.Bool, (this._enable)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("isOnce", Core.PropertyInfoType.Bool, (this._isOnce)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("isAndCheck", Core.PropertyInfoType.Bool, (this._isAndCheck)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("checkFuncNames", Core.PropertyInfoType.String, (from propertyEntry in this._checkFuncNames select (propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("execFuncNames", Core.PropertyInfoType.String, (from propertyEntry in this._execFuncNames select (propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("checkCallbackDataElements", Core.PropertyInfoType.EntityPtr, (from propertyEntry in this._checkCallbackDataElements select getEntityAddress(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("execCallbackDataElements", Core.PropertyInfoType.EntityPtr, (from propertyEntry in this._execCallbackDataElements select getEntityAddress(propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "trapCategory":
                    this._trapCategory = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "trapPriority":
                    this._trapPriority = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "enable":
                    this._enable = (DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData));
                    break;
                case "isOnce":
                    this._isOnce = (DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData));
                    break;
                case "isAndCheck":
                    this._isAndCheck = (DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData));
                    break;
                case "checkFuncNames":
                    this._checkFuncNames = (from rawValue in DataSetUtils.GetDynamicArrayValues<string>(propertyData) select (rawValue)).ToList();
                    break;
                case "execFuncNames":
                    this._execFuncNames = (from rawValue in DataSetUtils.GetDynamicArrayValues<string>(propertyData) select (rawValue)).ToList();
                    break;
                case "checkCallbackDataElements":
                    this._checkCallbackDataElements = (from rawValue in DataSetUtils.GetDynamicArrayValues<ulong>(propertyData) select initFunctions.GetEntityFromAddress(rawValue)).ToList();
                    break;
                case "execCallbackDataElements":
                    this._execCallbackDataElements = (from rawValue in DataSetUtils.GetDynamicArrayValues<ulong>(propertyData) select initFunctions.GetEntityFromAddress(rawValue)).ToList();
                    break;
            }
        }
    }
}
