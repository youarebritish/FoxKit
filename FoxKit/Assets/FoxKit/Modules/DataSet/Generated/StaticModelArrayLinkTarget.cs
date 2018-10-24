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

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class StaticModelArrayLinkTarget : Data
    {
        [SerializeField, Modules.DataSet.Property("StaticModelArrayLinkTarget")]
        private Entity _staticModelArray;

        [SerializeField, Modules.DataSet.Property("StaticModelArrayLinkTarget")]
        private uint _arrayIndex;

        /// <inheritdoc />
        public override short ClassId => 72;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("staticModelArray", Core.PropertyInfoType.EntityHandle, getEntityAddress(this._staticModelArray)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("arrayIndex", Core.PropertyInfoType.UInt32, (this._arrayIndex)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "staticModelArray":
                    this._staticModelArray = initFunctions.GetEntityFromAddress(DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData));
                    break;
                case "arrayIndex":
                    this._arrayIndex = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
            }
        }
    }
}
