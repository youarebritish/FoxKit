using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Structs;
using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public class TransformEntity : DataElement<TransformData>
    {
        public Vector3 Translation;        
        public Quaternion Rotation;
        public Vector3 Scale;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            if (propertyData.Name == "transform_translation")
            {
                var foxTranslation = DataSetUtils.GetStaticArrayPropertyValue<FoxVector3>(propertyData);
                Translation = DataSetUtils.FoxToolToUnity(foxTranslation);
            }
            else if (propertyData.Name == "transform_rotation_quat")
            {
                var foxRotation = DataSetUtils.GetStaticArrayPropertyValue<FoxQuat>(propertyData);
                Rotation = DataSetUtils.FoxToolToUnity(foxRotation);
            }
            else if (propertyData.Name == "transform_scale")
            {
                var foxScale = DataSetUtils.GetStaticArrayPropertyValue<FoxVector3>(propertyData);
                Scale = DataSetUtils.FoxToolToUnity(foxScale);
            }
        }
    }
}