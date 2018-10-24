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
    public class TppPermanentGimmickImportantBreakableParameter : DataElement
    {
        [SerializeField, Modules.DataSet.Property("TppPermanentGimmickImportantBreakableParameter")]
        private uint _life;

        [SerializeField, Modules.DataSet.Property("TppPermanentGimmickImportantBreakableParameter")]
        private uint _flag1;

        [SerializeField, Modules.DataSet.Property("TppPermanentGimmickImportantBreakableParameter")]
        private uint _flag2;

        /// <inheritdoc />
        public override short ClassId => 40;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("life", Core.PropertyInfoType.UInt32, (this._life)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("flag1", Core.PropertyInfoType.UInt32, (this._flag1)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("flag2", Core.PropertyInfoType.UInt32, (this._flag2)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "life":
                    this._life = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "flag1":
                    this._flag1 = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "flag2":
                    this._flag2 = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
            }
        }
    }
}
