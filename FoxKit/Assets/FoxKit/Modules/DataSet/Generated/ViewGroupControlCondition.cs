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
    public class ViewGroupControlCondition : Data
    {
        [SerializeField, Modules.DataSet.Property("ViewGroupControlCondition")]
        private uint _flags;

        [SerializeField, Modules.DataSet.Property("ViewGroupControlCondition")]
        private int _condition;

        [SerializeField, Modules.DataSet.Property("ViewGroupControlCondition")]
        private string _identify = string.Empty;

        /// <inheritdoc />
        public override short ClassId => 76;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("flags", Core.PropertyInfoType.UInt32, (this._flags)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("condition", Core.PropertyInfoType.Int32, (this._condition)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("identify", Core.PropertyInfoType.String, (this._identify)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "flags":
                    this._flags = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "condition":
                    this._condition = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "identify":
                    this._identify = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
            }
        }
    }
}
