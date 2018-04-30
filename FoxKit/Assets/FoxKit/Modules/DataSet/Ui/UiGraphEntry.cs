namespace FoxKit.Modules.DataSet.Ui
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEngine;
    
    /// <inheritdoc />
    /// <summary>
    /// TODO: Figure out.
    /// </summary>
    [Serializable]
    public class UiGraphEntry : Data
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private List<UnityEngine.Object> files = new List<UnityEngine.Object>();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private List<UnityEngine.Object> rawFiles = new List<UnityEngine.Object>();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private List<string> filesPaths = new List<string>();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private List<string> rawFilesPaths = new List<string>();

        /// <inheritdoc />
        protected override short ClassId => 96;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            foreach (var path in this.filesPaths)
            {
                UnityEngine.Object file = null;
                tryGetAsset(path, out file);
                this.files.Add(file);
            }

            foreach (var path in this.rawFilesPaths)
            {
                UnityEngine.Object file = null;
                tryGetAsset(path, out file);
                this.rawFiles.Add(file);
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
                        var filePtrList = DataSetUtils.GetDynamicArrayValues<FoxFilePtr>(propertyData);
                        this.files = new List<UnityEngine.Object>(filePtrList.Count);
                        this.filesPaths = new List<string>(filePtrList.Count);

                        foreach (var filePtr in filePtrList)
                        {
                            var path = DataSetUtils.ExtractFilePath(filePtr);
                            this.filesPaths.Add(path);
                        }
                        break;
                    }
                case "rawFiles":
                    {
                        var filePtrList = DataSetUtils.GetDynamicArrayValues<FoxFilePtr>(propertyData);
                        this.rawFiles = new List<UnityEngine.Object>(filePtrList.Count);
                        this.rawFilesPaths = new List<string>(filePtrList.Count);

                        foreach (var filePtr in filePtrList)
                        {
                            var path = DataSetUtils.ExtractFilePath(filePtr);
                            this.rawFilesPaths.Add(path);
                        }
                        break;
                    }
            }
        }
    }
}