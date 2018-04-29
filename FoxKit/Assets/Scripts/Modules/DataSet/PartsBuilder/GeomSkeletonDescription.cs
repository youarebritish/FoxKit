using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System;

namespace FoxKit.Modules.DataSet.PartsBuilder
{
    [Serializable]
    public class ModelDescription : PartDescription
    {
        public UnityEngine.Object GsklFile;
        public string GsklFilePath;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "gsklFile")
            {
                GsklFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
            base.OnAssetsImported(tryGetImportedAsset);

            tryGetImportedAsset(GsklFilePath, out GsklFile);
        }        
    }
}