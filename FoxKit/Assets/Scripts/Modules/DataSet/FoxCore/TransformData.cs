using System;
using System.Collections.Generic;

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
        public bool InheritTransform;
        public bool Visibility;
        public bool Selection;        
    }

    /// <summary>
    /// Bit
    /// </summary>
    public enum TransformData_Flags : uint
    {
        ENABLE_VISIBILITY = 1u,
        ENABLE_SELECTION = 2u,
        ENABLE_INHERIT_TRANSFORM = 4u
    }
}