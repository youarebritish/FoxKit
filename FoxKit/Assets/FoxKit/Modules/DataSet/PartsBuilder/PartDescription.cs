namespace FoxKit.Modules.DataSet.PartsBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Structs;
    using FoxTool.Fox.Types.Values;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.UI;

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
        [SerializeField]
        private List<EntityLink> depends = new List<EntityLink>();

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private string partName = string.Empty;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField]
        private string buildType = string.Empty;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MeshFilter)).image as Texture2D;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            foreach (var link in this.depends)
            {
                link.ResolveReference(tryGetAsset);
            }
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "depends":
                    this.depends = (from link in DataSetUtils.GetDynamicArrayValues<FoxEntityLink>(propertyData)
                                    select DataSetUtils.MakeEntityLink(this.DataSet, link))
                                    .ToList();
                    break;
                case "partName":
                    this.partName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "buildType":
                    this.buildType = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
            }
        }
    }
}