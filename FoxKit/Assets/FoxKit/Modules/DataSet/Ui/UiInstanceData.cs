namespace FoxKit.Modules.DataSet.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

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
        [SerializeField]
        private ObjectStringMap createWindowParams = new ObjectStringMap();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string windowFactoryName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private StringStringMap createWindowParamsPaths = new StringStringMap();

        /// <inheritdoc />
        protected override short ClassId => 120;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
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
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "files":
                    {
                        foreach (var entry in DataSetUtils.GetStringMap<FoxFilePtr>(propertyData))
                        {
                            this.createWindowParamsPaths.Add(entry.Key.ToString(), DataSetUtils.ExtractFilePath(entry.Value));
                        }

                        break;
                    }
                case "windowFactoryName":
                    {
                        this.windowFactoryName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                        break;
                    }
            }
        }
    }
}