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

    // Automatically generated from file gntn_common_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class GeoxTextureMaterialRegister : Data
    {
        [SerializeField, Modules.DataSet.Property("GeoxTextureMaterialRegister")]
        private FoxCore.EntityLink _materialLink;

        [SerializeField, Modules.DataSet.Property("GeoxTextureMaterialRegister")]
        private string _collisionMaterialName = string.Empty;

        [SerializeField, Modules.DataSet.Property("GeoxTextureMaterialRegister")]
        private string _collisionColorName = string.Empty;

        /// <inheritdoc />
        public override short ClassId => 200;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("materialLink", Core.PropertyInfoType.EntityLink, convertEntityLink(this._materialLink)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("collisionMaterialName", Core.PropertyInfoType.String, (this._collisionMaterialName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("collisionColorName", Core.PropertyInfoType.String, (this._collisionColorName)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "materialLink":
                    this._materialLink = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData));
                    break;
                case "collisionMaterialName":
                    this._collisionMaterialName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "collisionColorName":
                    this._collisionColorName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
            }
        }
    }
}
