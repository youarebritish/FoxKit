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
    public class TppRequestWeatherTagTrapExecDataElement : DataElement
    {
        [SerializeField, Modules.DataSet.Property("TppRequestWeatherTagTrapExecDataElement")]
        private string _funcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("TppRequestWeatherTagTrapExecDataElement")]
        private string _tagName = string.Empty;

        [SerializeField, Modules.DataSet.Property("TppRequestWeatherTagTrapExecDataElement")]
        private byte _priority;

        [SerializeField, Modules.DataSet.Property("TppRequestWeatherTagTrapExecDataElement")]
        private float _interpTime;

        /// <inheritdoc />
        public override short ClassId => 44;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("funcName", Core.PropertyInfoType.String, (this._funcName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("tagName", Core.PropertyInfoType.String, (this._tagName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("priority", Core.PropertyInfoType.UInt8, (this._priority)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("interpTime", Core.PropertyInfoType.Float, (this._interpTime)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "funcName":
                    this._funcName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "tagName":
                    this._tagName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "priority":
                    this._priority = (DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData));
                    break;
                case "interpTime":
                    this._interpTime = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
            }
        }
    }
}
