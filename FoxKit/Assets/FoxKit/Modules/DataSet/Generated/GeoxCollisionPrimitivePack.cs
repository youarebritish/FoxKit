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
    public class GeoxCollisionPrimitivePack : TransformData
    {
        [SerializeField, Modules.DataSet.Property("GeoxCollisionPrimitivePack")]
        private UnityEngine.Object _geomFile;

        [SerializeField, HideInInspector]
        private string geomFilePath;

        /// <inheritdoc />
        public override short ClassId => 320;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.geomFilePath, out this._geomFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("geomFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._geomFile)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "geomFile":
                    this.geomFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
            }
        }
    }
}
