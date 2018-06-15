namespace FoxKit.Modules.DataSet.Sdx
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
        public override short ClassId => 96;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.soundDataFilePath, out this.soundDataFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("soundDataFile", Core.PropertyInfoType.FilePtr, DataSetUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.soundDataFile))));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("syncLoad", Core.PropertyInfoType.Bool, this.syncLoad));

            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "soundDataFile":
                    this.soundDataFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "syncLoad":
                    this.syncLoad = DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData);
                    break;
            }
        }
    }
}