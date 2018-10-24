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
    public class ShearTransformEntity : DataElement
    {
        [SerializeField, Modules.DataSet.Property("ShearTransformEntity")]
        private UnityEngine.Vector3 _shearTransform_shear;

        /// <inheritdoc />
        public override short ClassId => 48;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("shearTransform_shear", Core.PropertyInfoType.Vector3, FoxUtils.UnityToFox(this._shearTransform_shear)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "shearTransform_shear":
                    this._shearTransform_shear = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Vector3>(propertyData));
                    break;
            }
        }
    }
}
