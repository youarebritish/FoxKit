using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using FoxKit.Utils;
using System;
using FoxKit.Modules.DataSet.FoxCore;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.Ui
{
    [Serializable]
    public class UiGraphEntry : Data
    {
        public List<UnityEngine.Object> Files;
        public List<UnityEngine.Object> RawFiles;

        public List<string> FilesPaths;
        public List<string> RawFilesPaths;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "files")
            {
                var filePtrList = DataSetUtils.GetDynamicArrayValues<FoxFilePtr>(propertyData);
                Files = new List<UnityEngine.Object>(filePtrList.Count);
                FilesPaths = new List<string>(filePtrList.Count);

                foreach (var filePtr in filePtrList)
                {
                    var path = DataSetUtils.ExtractFilePath(filePtr);
                    FilesPaths.Add(path);
                }
            }
            else if (propertyData.Name == "rawFiles")
            {
                var filePtrList = DataSetUtils.GetDynamicArrayValues<FoxFilePtr>(propertyData);
                RawFiles = new List<UnityEngine.Object>(filePtrList.Count);
                RawFilesPaths = new List<string>(filePtrList.Count);

                foreach (var filePtr in filePtrList)
                {
                    var path = DataSetUtils.ExtractFilePath(filePtr);
                    RawFilesPaths.Add(path);
                }
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            foreach(var path in FilesPaths)
            {
                UnityEngine.Object file = null;
                tryGetAsset(path, out file);
                Files.Add(file);
            }

            foreach (var path in RawFilesPaths)
            {
                UnityEngine.Object file = null;
                tryGetAsset(path, out file);
                RawFiles.Add(file);
            }
        }
    }
}