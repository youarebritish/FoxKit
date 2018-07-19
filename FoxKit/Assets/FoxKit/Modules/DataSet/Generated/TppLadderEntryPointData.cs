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

    // Automatically generated from file afgh_field_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppLadderEntryPointData : Data
    {
        [SerializeField, Modules.DataSet.Property("TppLadderEntryPointData")]
        private int _entryType;

        [SerializeField, Modules.DataSet.Property("TppLadderEntryPointData")]
        private uint _locateStep;

        /// <inheritdoc />
        public override short ClassId => 72;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("entryType", Core.PropertyInfoType.Int32, (this._entryType)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("locateStep", Core.PropertyInfoType.UInt32, (this._locateStep)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "entryType":
                    this._entryType = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "locateStep":
                    this._locateStep = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
            }
        }
    }
}
