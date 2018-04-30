namespace FoxKit.Modules.DataSet.TppGameKit
{
    using System;

    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    /// <inheritdoc />
    /// <summary>
    /// Mortar parameters for <see cref="TppPermanentGimmickData"/>.
    /// </summary>
    [Serializable]
    public class TppPermanentGimmickMortarParameter : TppPermanentGimmickParameter
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        private float rotationLimitLeftRight;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        private float rotationLimitUp;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        private float rotationLimitDown;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        private UnityEngine.Object defaultShellPartsFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        private UnityEngine.Object flareShellPartsFile;

        /// <summary>
        /// Path to <see cref="defaultShellPartsFile"/>.
        /// </summary>
        private string defaultShellPartsFilePath;

        /// <summary>
        /// Path to <see cref="flareShellPartsFile"/>.
        /// </summary>
        private string flareShellPartsFilePath;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.defaultShellPartsFilePath, out this.defaultShellPartsFile);
            tryGetAsset(this.flareShellPartsFilePath, out this.flareShellPartsFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "rotationLimitLeftRight":
                    this.rotationLimitLeftRight = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "rotationLimitUp":
                    this.rotationLimitUp = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "rotationLimitDown":
                    this.rotationLimitDown = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "defaultShellPartsFile":
                    this.defaultShellPartsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "flareShellPartsFile":
                    this.flareShellPartsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
            }
        }
    }
}