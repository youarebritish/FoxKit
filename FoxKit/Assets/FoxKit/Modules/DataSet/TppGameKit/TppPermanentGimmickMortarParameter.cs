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
        [SerializeField]
        private float rotationLimitLeftRight;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float rotationLimitUp;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float rotationLimitDown;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object defaultShellPartsFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object flareShellPartsFile;

        /// <summary>
        /// Path to <see cref="defaultShellPartsFile"/>.
        /// </summary>
        [SerializeField]
        private string defaultShellPartsFilePath;

        /// <summary>
        /// Path to <see cref="flareShellPartsFile"/>.
        /// </summary>
        [SerializeField]
        private string flareShellPartsFilePath;

        /// <inheritdoc />
        public override short ClassId => 88;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.defaultShellPartsFilePath, out this.defaultShellPartsFile);
            tryGetAsset(this.flareShellPartsFilePath, out this.flareShellPartsFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty("rotationLimitLeftRight", Core.PropertyInfoType.Float, this.rotationLimitLeftRight));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty("rotationLimitUp", Core.PropertyInfoType.Float, this.rotationLimitUp));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty("rotationLimitDown", Core.PropertyInfoType.Float, this.rotationLimitDown));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "defaultShellPartsFile",
                    Core.PropertyInfoType.FilePtr,
                    FoxUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.defaultShellPartsFile))));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "flareShellPartsFile",
                    Core.PropertyInfoType.FilePtr,
                    FoxUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.defaultShellPartsFile))));

            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "rotationLimitLeftRight":
                    this.rotationLimitLeftRight = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "rotationLimitUp":
                    this.rotationLimitUp = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "rotationLimitDown":
                    this.rotationLimitDown = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "defaultShellPartsFile":
                    this.defaultShellPartsFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "flareShellPartsFile":
                    this.flareShellPartsFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
            }
        }
    }
}