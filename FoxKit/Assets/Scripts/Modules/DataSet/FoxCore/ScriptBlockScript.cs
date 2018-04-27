using System;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;
using System.IO;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public class ScriptBlockScript : Data
    {
        public UnityEngine.Object Script;
        public string ScriptPath;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "script")
            {
                ScriptPath = Path.GetFileName(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData).ToString());
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAsset tryGetImportedAsset)
        {
            base.OnAssetsImported(tryGetImportedAsset);

            if (tryGetImportedAsset(ScriptPath, out Script))
            {

            }
        }
    }
}