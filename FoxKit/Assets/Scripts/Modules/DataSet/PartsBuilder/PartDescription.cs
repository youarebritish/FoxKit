using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;
using System.Collections.Generic;
using System.Linq;

namespace FoxKit.Modules.DataSet.PartsBuilder
{
    public abstract class PartDescription : Data
    {
        public List<UnityEngine.Object> Depends;
        public string PartName;
        public string BuildType;

        public List<EntityLink> DependsEntityLinks;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "depends")
            {
                DependsEntityLinks = (from link in DataSetUtils.GetDynamicArrayValues<FoxEntityLink>(propertyData)
                                     select DataSetUtils.MakeEntityLink(DataSet, link))
                                     .ToList();
            }
            else if (propertyData.Name == "partName")
            {
                PartName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
            else if (propertyData.Name == "buildType")
            {
                BuildType = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
            foreach(var link in DependsEntityLinks)
            {
                link.ResolveReference(tryGetImportedAsset);
            }
        }
    }
}