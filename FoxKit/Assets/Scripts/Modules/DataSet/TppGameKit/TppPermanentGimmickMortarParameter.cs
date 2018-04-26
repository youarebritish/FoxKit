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
                var filePtr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(filePtr, out DefaultShellPartsFile);
            }
            else if (propertyData.Name == "flareShellPartsFile")
            {
                var filePtr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(filePtr, out FlareShellPartsFile);
            }
        }
    }
}