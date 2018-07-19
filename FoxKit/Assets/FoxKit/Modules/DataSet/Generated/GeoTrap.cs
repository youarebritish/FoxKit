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

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class GeoTrap : TransformData
    {
        [SerializeField, Modules.DataSet.Property("GeoTrap")]
        private List<FoxCore.EntityLink> _conditionArray;

        [SerializeField, Modules.DataSet.Property("GeoTrap")]
        private bool _enable;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("conditionArray", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._conditionArray select convertEntityLink(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("enable", Core.PropertyInfoType.Bool, (this._enable)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "conditionArray":
                    this._conditionArray = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
                case "enable":
                    this._enable = (DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData));
                    break;
            }
        }
    }
}
