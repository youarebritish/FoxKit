namespace FoxKit.Modules.DataSet.Sdx
{
    using System;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// TODO: Figure out what this is.
    /// </summary>
    [Serializable]
    public class SoundPackage : Data
    {
        /// <summary>
        /// TODO: Figure out what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Sound Description")]
        private UnityEngine.Object soundDataFile;

        /// <summary>
        /// TODO: Figure out what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Sound Description")]
        private bool syncLoad;

        /// <summary>
        /// File path for <see cref="soundDataFile"/>.
        /// </summary>
        [SerializeField, HideInInspector]
        private string soundDataFilePath;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(AudioClip)).image as Texture2D;

        /// <inheritdoc />
        protected override short ClassId => 96;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.soundDataFilePath, out this.soundDataFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "soundDataFile":
                    this.soundDataFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "syncLoad":
                    this.syncLoad = DataSetUtils.GetStaticArrayPropertyValue<FoxBool>(propertyData).Value;
                    break;
            }
        }
    }
}