namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;

    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// The script block script.
    /// </summary>
    [Serializable]
    public class ScriptBlockScript : Data
    {
        /// <summary>
        /// The Lua script to execute when the ScriptBlock activates.
        /// </summary>
        [SerializeField, Category("ScriptBlockScript")]
        private UnityEngine.Object script;

        /// <summary>
        /// The script path.
        /// </summary>
        [SerializeField, HideInInspector]
        private string scriptPath;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MonoScript)).image as Texture2D;

        /// <inheritdoc />
        protected override short ClassId => 88;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.scriptPath, out this.script);
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "script")
            {
                this.scriptPath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
        }
    }
}