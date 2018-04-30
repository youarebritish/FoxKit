using System;
using System.Collections.Generic;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;
using System.Linq;

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

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);
            
            if (propertyData.Name == "parent")
            {
                var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityHandle>(propertyData).Handle;
                Parent = initFunctions.GetEntityFromAddress(address) as TransformData;
            }
            else if (propertyData.Name == "transform")
            {
                var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                Transform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                if (Transform != null)
                {
                    Transform.Owner = this;
                }
            }
            else if (propertyData.Name == "shearTransform")
            {
                var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                ShearTransform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                if (ShearTransform != null)
                {
                    ShearTransform.Owner = this;
                }
            }
            else if (propertyData.Name == "pivotTransform")
            {
                var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                PivotTransform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                if (PivotTransform != null)
                {
                    PivotTransform.Owner = this;
                }
            }
            else if (propertyData.Name == "children")
            {
                Children = (from handle in DataSetUtils.GetListValues<FoxEntityHandle>(propertyData)
                           select initFunctions.GetEntityFromAddress(handle.Handle) as TransformData)
                           .ToList();
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