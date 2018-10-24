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

    // Automatically generated from file afgh_field_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class FoxTrapExecViewGroupControlCallbackDataElement : DataElement
    {
        [SerializeField, Modules.DataSet.Property("FoxTrapExecViewGroupControlCallbackDataElement")]
        private string _funcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("FoxTrapExecViewGroupControlCallbackDataElement")]
        private string _identify = string.Empty;

        /// <inheritdoc />
        public override short ClassId => 36;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("funcName", Core.PropertyInfoType.String, (this._funcName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("identify", Core.PropertyInfoType.String, (this._identify)));
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
                case "identify":
                    this._identify = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
            }
        }
    }
}
