using System;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using FoxKit.Utils;

namespace FoxKit.Modules.DataSet.TppGameKit
{
    [Serializable]
    public class TppPermanentGimmickMortarParameter : TppPermanentGimmickParameter
    {
        public float RotationLimitLeftRight;
        public float RotationLimitUp;
        public float RotationLimitDown;
        public UnityEngine.Object DefaultShellPartsFile;
        public UnityEngine.Object FlareShellPartsFile;

        public string DefaultShellPartsFilePath;
        public string FlareShellPartsFilePath;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "rotationLimitLeftRight")
            {
                RotationLimitLeftRight = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "rotationLimitUp")
            {
                RotationLimitUp = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "rotationLimitDown")
            {
                RotationLimitDown = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "defaultShellPartsFile")
            {
                DefaultShellPartsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "flareShellPartsFile")
            {
                FlareShellPartsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
            base.OnAssetsImported(tryGetImportedAsset);
            tryGetImportedAsset(DefaultShellPartsFilePath, out DefaultShellPartsFile);
            tryGetImportedAsset(FlareShellPartsFilePath, out FlareShellPartsFile);
        }
    }
}