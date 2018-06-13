namespace FoxKit.Modules.DataSet.Ui
{
    using System;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// TODO: Figure out.
    /// </summary>
    [Serializable]
    public class UiInstanceData : Data
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("UI")]
        private ObjectStringMap createWindowParams = new ObjectStringMap();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("UI")]
        private string windowFactoryName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, HideInInspector]
        private StringStringMap createWindowParamsPaths = new StringStringMap();

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Canvas)).image as Texture2D;

        /// <inheritdoc />
        protected override short ClassId => 120;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            foreach (var path in this.createWindowParamsPaths)
            {
                UnityEngine.Object file;
                tryGetAsset(path.Value, out file);
                this.createWindowParams.Add(path.Key, file);
            }
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "files":
                    {
                        foreach (var entry in DataSetUtils.GetStringMap<string>(propertyData))
                        {
                            this.createWindowParamsPaths.Add(entry.Key, DataSetUtils.ExtractFilePath(entry.Value));
                        }

                        break;
                    }
                case "windowFactoryName":
                    {
                        this.windowFactoryName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                        break;
                    }
            }
        }
    }
}