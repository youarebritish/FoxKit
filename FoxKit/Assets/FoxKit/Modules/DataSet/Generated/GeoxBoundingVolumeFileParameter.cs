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
    public class GeoxBoundingVolumeFileParameter : Data
    {
        [SerializeField, Modules.DataSet.Property("GeoxBoundingVolumeFileParameter")]
        private UnityEngine.Vector3 _gridSize;

        /// <inheritdoc />
        public override short ClassId => 80;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("gridSize", Core.PropertyInfoType.Vector3, FoxUtils.UnityToFox(this._gridSize)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "gridSize":
                    this._gridSize = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Vector3>(propertyData));
                    break;
            }
        }
    }
}
