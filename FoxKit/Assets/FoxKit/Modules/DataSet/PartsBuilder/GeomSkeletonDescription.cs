namespace FoxKit.Modules.DataSet.PartsBuilder
{
    using System;

    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

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
        [SerializeField, DataSet.Property("GeomSkeleton Description")]
        private UnityEngine.Object gsklFile;

        /// <summary>
        /// The GeoGsklFile file path.
        /// </summary>
        [SerializeField, HideInInspector]
        private string gsklFilePath;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(CharacterJoint)).image as Texture2D;

        /// <inheritdoc />
        protected override short ClassId => 112;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.gsklFilePath, out this.gsklFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "gsklFile")
            {
                this.gsklFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
            }
        }
    }
}