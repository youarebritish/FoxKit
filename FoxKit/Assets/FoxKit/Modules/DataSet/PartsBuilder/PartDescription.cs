namespace FoxKit.Modules.DataSet.PartsBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Base class for a PartsFile Data Entity.
    /// </summary>
    [Serializable]
    public abstract class PartDescription : Data
    {
        /// <summary>
        /// Other PartDescription Entities that this depends on.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Part Description")]
        private List<EntityLink> depends = new List<EntityLink>();

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Part Description")]
        private string partName = string.Empty;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Part Description")]
        private string buildType = string.Empty;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MeshFilter)).image as Texture2D;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            foreach (var link in this.depends)
            {
                link.ResolveReference(tryGetAsset);
            }
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "depends":
                    this.depends = (from link in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData)
                                    select DataSetUtils.MakeEntityLink(this.DataSet, link))
                                    .ToList();
                    break;
                case "partName":
                    this.partName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "buildType":
                    this.buildType = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
            }
        }
    }
}