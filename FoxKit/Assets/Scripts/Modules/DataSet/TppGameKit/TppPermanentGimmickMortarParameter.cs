using FoxKit.Modules.DataSet.FoxCore;
using System;
using FoxKit.Modules.DataSet.Importer;
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
        public UnityEngine.Object DefaultShellPartsFile;    // TODO
        public UnityEngine.Object FlareShellPartsFile;      // TODO

        protected override void ReadProperty(FoxProperty propertyData, EntityFactory.GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            if (propertyData.Name == "rotationLimitLeftRight")
            {
                RotationLimitLeftRight = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "rotationLimitUp")
            {
                RotationLimitUp = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            if (propertyData.Name == "rotationLimitDown")
            {
                RotationLimitDown = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
        }
    }
}