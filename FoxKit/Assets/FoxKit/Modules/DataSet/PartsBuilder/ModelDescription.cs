using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace FoxKit.Modules.DataSet.PartsBuilder
{
    public class ModelDescription : PartDescription
    {
        public UnityEngine.Object ModelFile;
        public UnityEngine.Object ConnectPointFile;
        public UnityEngine.Object GameRigFile;
        public UnityEngine.Object HelpBoneFile;
        public UnityEngine.Object LipAdjustBinaryFile;
        public UnityEngine.Object FacialSettingFile;

        public List<string> InvisibleMeshNames;
        public float LodFarPixelSize;
        public float LodNearPixelSize;
        public float LodPolygonSize;

        public ModelDescription_DrawRejectionLevel DrawRejectionLevel = ModelDescription_DrawRejectionLevel.DEFAULT;
        public ModelDescription_RejectFarRangeShadowCast RejectFarRangeShadowCast = ModelDescription_RejectFarRangeShadowCast.DEFAULT;

        public string ModelFilePath;
        public string ConnectPointFilePath;
        public string GameRigFilePath;
        public string HelpBoneFilePath;
        public string LipAdjustBinaryFilePath;
        public string FacialSettingFilePath;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "modelFile")
            {
                ModelFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "connectPointFile")
            {
                ConnectPointFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "gameRigFile")
            {
                GameRigFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "helpBoneFile")
            {
                HelpBoneFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "lipAdjustBinaryFile")
            {
                LipAdjustBinaryFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "facialSettingFile")
            {
                FacialSettingFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "invisibleMeshNames")
            {
                InvisibleMeshNames = (from name in DataSetUtils.GetDynamicArrayValues<FoxString>(propertyData)
                                      select name.ToString()).ToList();
            }
            else if (propertyData.Name == "lodFarPixelSize")
            {
                LodFarPixelSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "lodNearPixelSize")
            {
                LodNearPixelSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "lodPolygonSize")
            {
                LodPolygonSize = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "drawRejectionLevel")
            {
                DrawRejectionLevel = (ModelDescription_DrawRejectionLevel)DataSetUtils.GetStaticArrayPropertyValue<FoxInt32>(propertyData).Value;
            }
            else if (propertyData.Name == "rejectFarRangeShadowCast")
            {
                RejectFarRangeShadowCast = (ModelDescription_RejectFarRangeShadowCast)DataSetUtils.GetStaticArrayPropertyValue<FoxInt32>(propertyData).Value;
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(ModelFilePath, out ModelFile);
            tryGetAsset(ConnectPointFilePath, out ConnectPointFile);
            tryGetAsset(GameRigFilePath, out GameRigFile);
            tryGetAsset(HelpBoneFilePath, out HelpBoneFile);
            tryGetAsset(LipAdjustBinaryFilePath, out LipAdjustBinaryFile);
            tryGetAsset(FacialSettingFilePath, out FacialSettingFile);
        }

        public enum ModelDescription_DrawRejectionLevel
        {
            [Description("LEVEL0(size:0.5m)")]
            Level0 = 0,
            [Description("LEVEL1(size:1m)")]
            Level1 = 1,
            [Description("LEVEL2(size:2m)")]
            Level2 = 2,
            [Description("LEVEL3(size:4m)")]
            Level3 = 3,
            [Description("LEVEL4(size:8m)")]
            Level4 = 4,
            [Description("LEVEL5(size:16m)")]
            Level5 = 5,
            [Description("LEVEL6(size:32m)")]
            Level6 = 6,
            [Description("NO_REJECT")]
            NO_REJECT = 7,
            [Description("DEFAULT")]
            DEFAULT = 8
        }

        public enum ModelDescription_RejectFarRangeShadowCast
        {
            NO_REJECT = 0,
            REJECT = 1,
            DEFAULT = 2
        }
    }
}