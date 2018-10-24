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
    public class TerrainDecalPack : Data
    {
        [SerializeField, Modules.DataSet.Property("TerrainDecalPack")]
        private UnityEngine.Object _terrainDecalPackFile;

        [SerializeField, HideInInspector]
        private string terrainDecalPackFilePath;

        [SerializeField, Modules.DataSet.Property("TerrainDecalPack")]
        private List<FoxCore.EntityLink> _materialLinks;

        /// <inheritdoc />
        public override short ClassId => 104;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.terrainDecalPackFilePath, out this._terrainDecalPackFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("terrainDecalPackFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._terrainDecalPackFile)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("materialLinks", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._materialLinks select convertEntityLink(propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "terrainDecalPackFile":
                    this.terrainDecalPackFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "materialLinks":
                    this._materialLinks = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
            }
        }
    }
}
