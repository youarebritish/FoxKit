namespace FoxKit.Modules.DataSet.TppGameKit
{
    using System;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// Note: Gimmicks are weird. They're Data, not TransformData, and get their transform from an lba.
    /// </summary>
    [Serializable]
    public class TppPermanentGimmickData : Data
    {
        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Category("Gimmick")]
        private UnityEngine.Object partsFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Category("Gimmick")]
        private UnityEngine.Object locatorFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Category("Gimmick")]
        private TppPermanentGimmickParameter parameters;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Category("Gimmick")]
        private uint flags1 = 5;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Category("Gimmick")]
        private uint flags2;

        /// <summary>
        /// Path to <see cref="partsFile"/>.
        /// </summary>
        [SerializeField]
        private string partsFilePath = string.Empty;

        /// <summary>
        /// Path to <see cref="locatorFile"/>.
        /// </summary>
        [SerializeField]
        private string locatorFilePath = string.Empty;

        /// <inheritdoc />
        protected override short ClassId => 144;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.partsFilePath, out this.partsFile);
            tryGetAsset(this.locatorFilePath, out this.locatorFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "partsFile":
                    this.partsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "locaterFile":
                    this.locatorFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "parameters":
                    var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                    this.parameters = initFunctions.GetEntityFromAddress(address) as TppPermanentGimmickParameter;

                    Assert.IsNotNull(this.parameters);
                    this.parameters.Owner = this;
                    break;
                case "flags1":
                    this.flags1 = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
                    break;
                case "flags2":
                    this.flags2 = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
                    break;
            }
        }
    }
}