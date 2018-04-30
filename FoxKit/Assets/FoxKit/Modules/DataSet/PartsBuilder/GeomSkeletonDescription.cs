namespace FoxKit.Modules.DataSet.PartsBuilder
{
    using System;

    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Not sure what this is. Links a GeoGsklFile file to a <see cref="T:FoxKit.Modules.DataSet.PartsBuilder.ModelDescription" />.
    /// </summary>
    [Serializable]
    public class GeomSkeletonDescription : PartDescription
    {
        /// <summary>
        /// The GeoGsklFile file.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object gsklFile;

        /// <summary>
        /// The GeoGsklFile file path.
        /// </summary>
        [SerializeField]
        private string gsklFilePath;

        /// <inheritdoc />
        protected override short ClassId => 112;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.gsklFilePath, out this.gsklFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "gsklFile")
            {
                this.gsklFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
        }
    }
}