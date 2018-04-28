using FoxKit.Modules.DataSet.FoxCore;
using System;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.Sdx
{
    [Serializable]
    public class SoundPackage : Data
    {        
        public UnityEngine.Object SoundDataFile;
        public bool SyncLoad;
        public string SoundDataFilePath;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "soundDataFile")
            {
                SoundDataFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "syncLoad")
            {
                SyncLoad = DataSetUtils.GetStaticArrayPropertyValue<FoxBool>(propertyData).Value;
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
            base.OnAssetsImported(tryGetImportedAsset);
            tryGetImportedAsset(SoundDataFilePath, out SoundDataFile);
        }
    }
}