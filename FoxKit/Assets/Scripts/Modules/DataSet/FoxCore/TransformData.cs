using System;
using System.Collections.Generic;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

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

        // https://docs.unity3d.com/ScriptReference/EditorGUILayout.MaskField.html
        public bool InheritTransform;
        public bool Visibility;
        public bool Selection;

        protected override void ReadProperty(FoxProperty propertyData)
        {
            base.ReadProperty(propertyData);

            if (propertyData.Name == "parent")
            {

            }
            else if (propertyData.Name == "transform")
            {

            }
            else if (propertyData.Name == "shearTransform")
            {

            }
            else if (propertyData.Name == "pivotTransform")
            {

            }
            else if (propertyData.Name == "children")
            {

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