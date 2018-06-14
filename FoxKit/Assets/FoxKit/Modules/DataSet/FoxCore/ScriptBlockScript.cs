namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Utils;

    using FoxLib;

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
        [SerializeField, Modules.DataSet.Property("ScriptBlockScript")]
        private UnityEngine.Object script;

        /// <summary>
        /// The script path.
        /// </summary>
        [SerializeField, HideInInspector]
        private string scriptPath;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MonoScript)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 88;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress);
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "script",
                    Core.PropertyInfoType.FilePtr,
                    DataSetUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.script))));

            return parentProperties;
        }

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.scriptPath, out this.script);
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "script")
            {
                this.scriptPath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
            }
        }
    }
}