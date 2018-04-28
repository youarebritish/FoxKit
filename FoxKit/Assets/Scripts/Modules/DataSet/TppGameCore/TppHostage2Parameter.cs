using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using FoxKit.Utils;
using FoxKit.Modules.DataSet.GameCore;
using FoxKit.Utils.UI.StringMap;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    public class TppHostage2Parameter : GameObjectParameter
    {
        public UnityEngine.Object PartsFile;
        public UnityEngine.Object MotionGraphFile;
        public UnityEngine.Object MtarFile;
        public UnityEngine.Object ExtensionMtarFile;
        public ObjectStringMap VfxFiles;

        public string PartsFilePath;
        public string MotionGraphFilePath;
        public string MtarFilePath;
        public string ExtensionMtarFilePath;
        public StringStringMap VfxFilePaths;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);
            
            if (propertyData.Name == "partsFile")
            {
                PartsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "motionGraphFile")
            {
                MotionGraphFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "mtarFile")
            {
                MtarFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "extensionMtarFile")
            {
                ExtensionMtarFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "vfxFiles")
            {
                var dictionary = DataSetUtils.GetStringMap<FoxFilePtr>(propertyData);

                VfxFilePaths = new StringStringMap();
                foreach(var entry in dictionary)
                {
                    var path = ExtensionMtarFilePath = DataSetUtils.ExtractFilePath(entry.Value);
                    VfxFilePaths.Add(entry.Key.ToString(), path);
                }
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
            base.OnAssetsImported(tryGetImportedAsset);

            tryGetImportedAsset(PartsFilePath, out PartsFile);
            tryGetImportedAsset(MotionGraphFilePath, out MotionGraphFile);
            tryGetImportedAsset(MtarFilePath, out MtarFile);
            tryGetImportedAsset(ExtensionMtarFilePath, out ExtensionMtarFile);

            VfxFiles = new ObjectStringMap();
            foreach (var entry in VfxFilePaths)
            {
                UnityEngine.Object asset = null;
                tryGetImportedAsset(entry.Value, out asset);
                VfxFiles.Add(entry.Key, asset);
            }
        }
    }
}