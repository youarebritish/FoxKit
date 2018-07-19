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
    public class TppGimmickElectricCableLinkSetData : Data
    {
        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private FoxCore.EntityLink _electricCableData;

        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private FoxCore.EntityLink _poleData;

        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private List<string> _electricCable;

        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private List<string> _pole;

        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private List<byte> _cnpIndex;

        /// <inheritdoc />
        public override short ClassId => 176;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("electricCableData", Core.PropertyInfoType.EntityLink, convertEntityLink(this._electricCableData)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("poleData", Core.PropertyInfoType.EntityLink, convertEntityLink(this._poleData)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("electricCable", Core.PropertyInfoType.String, (from propertyEntry in this._electricCable select (propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("pole", Core.PropertyInfoType.String, (from propertyEntry in this._pole select (propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("cnpIndex", Core.PropertyInfoType.UInt8, (from propertyEntry in this._cnpIndex select (propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "electricCableData":
                    this._electricCableData = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData));
                    break;
                case "poleData":
                    this._poleData = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData));
                    break;
                case "electricCable":
                    this._electricCable = (from rawValue in DataSetUtils.GetDynamicArrayValues<string>(propertyData) select (rawValue)).ToList();
                    break;
                case "pole":
                    this._pole = (from rawValue in DataSetUtils.GetDynamicArrayValues<string>(propertyData) select (rawValue)).ToList();
                    break;
                case "cnpIndex":
                    this._cnpIndex = (from rawValue in DataSetUtils.GetDynamicArrayValues<byte>(propertyData) select (rawValue)).ToList();
                    break;
            }
        }
    }
}
