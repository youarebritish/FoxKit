namespace FoxKit.Modules.DataSet.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.Exporter;
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
        private OrderedDictionary_string_Object createWindowParams = new OrderedDictionary_string_Object();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("UI")]
        private string windowFactoryName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, HideInInspector]
        private OrderedDictionary_string_string createWindowParamsPaths = new OrderedDictionary_string_string();

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Canvas)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 120;

        /// <inheritdoc />
        public override ushort Version => 1;

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
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(
                PropertyInfoFactory.MakeStringMapProperty(
                    "createWindowParams",
                    Core.PropertyInfoType.FilePtr,
                    this.createWindowParams.ToDictionary(
                        entry => entry.Key,
                        entry => DataSetUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(entry.Value)) as object)));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "windowFactoryName",
                    Core.PropertyInfoType.String,
                    this.windowFactoryName));
            return parentProperties;
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
                        
                        //this.createWindowParamsPaths.AddRange(DataSetUtils.GetStringMap<string>(propertyData));

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