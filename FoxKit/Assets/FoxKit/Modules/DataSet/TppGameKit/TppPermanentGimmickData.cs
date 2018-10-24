namespace FoxKit.Modules.DataSet.TppGameKit
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

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
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private UnityEngine.Object partsFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private UnityEngine.Object locatorFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private TppPermanentGimmickParameter parameters;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private uint flags1 = 5;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
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
        public override short ClassId => 144;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.partsFilePath, out this.partsFile);
            tryGetAsset(this.locatorFilePath, out this.locatorFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty(
                "partsFile",
                Core.PropertyInfoType.FilePtr,
                FoxUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.partsFile))));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "locaterFile",
                    Core.PropertyInfoType.FilePtr,
                    FoxUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.locatorFile))));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "parameters",
                    Core.PropertyInfoType.EntityPtr,
                    getEntityAddress(this.parameters)));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty("flags1", Core.PropertyInfoType.UInt32, this.flags1));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty("flags2", Core.PropertyInfoType.UInt32, this.flags2));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "partsFile":
                    this.partsFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "locaterFile":
                    this.locatorFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "parameters":
                    var address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                    this.parameters = initFunctions.GetEntityFromAddress(address) as TppPermanentGimmickParameter;

                    //Assert.IsNotNull(this.parameters); This is sometimes null, why
                    if (this.parameters != null)
                    {
                        this.parameters.Owner = this;
                    }
                    break;
                case "flags1":
                    this.flags1 = DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData);
                    break;
                case "flags2":
                    this.flags2 = DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData);
                    break;
            }
        }
    }
}