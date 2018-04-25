using System;
using System.Collections.Generic;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;
using static FoxKit.Modules.DataSet.Importer.EntityFactory;

namespace FoxKit.Modules.DataSet.FoxCore
{
    /// <summary>
    /// Base class for Entities with a physical location in the world.
    /// </summary>
    [Serializable]
    public abstract class TransformData : Data
    {
        public TransformData Parent;
        public TransformEntity Transform;
        public TransformEntity ShearTransform;
        public TransformEntity PivotTransform;
        public List<TransformData> Children = new List<TransformData>();
        
        public bool InheritTransform = true;
        public bool Visibility = true;
        public bool Selection = true;

        protected override void ReadProperty(FoxProperty propertyData, GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            return;
            if (propertyData.Name == "parent")
            {
                /*var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityHandle>(propertyData).Handle;
                Parent = getEntity(address) as TransformData;*/
            }
            else if (propertyData.Name == "transform")
            {
                /*var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                Transform = getEntity(address) as TransformEntity;*/
            }
            else if (propertyData.Name == "shearTransform")
            {
                /*var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                ShearTransform = getEntity(address) as TransformEntity;*/
            }
            else if (propertyData.Name == "pivotTransform")
            {
                /*var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                PivotTransform = getEntity(address) as TransformEntity;*/
            }
            else if (propertyData.Name == "children")
            {
                // TODO List
            }
            else if (propertyData.Name == "flags")
            {
                TransformData_Flags flags = (TransformData_Flags)DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
                InheritTransform = flags.HasFlag(TransformData_Flags.ENABLE_INHERIT_TRANSFORM);
                Visibility = flags.HasFlag(TransformData_Flags.ENABLE_VISIBILITY);
                Selection = flags.HasFlag(TransformData_Flags.ENABLE_VISIBILITY);
            }
        }
    }

    /// <summary>
    /// Bit flags for TransformData
    /// </summary>
    [Flags]
    public enum TransformData_Flags : uint
    {
        ENABLE_VISIBILITY = 1u,
        ENABLE_SELECTION = 2u,
        ENABLE_INHERIT_TRANSFORM = 4u
    }
}