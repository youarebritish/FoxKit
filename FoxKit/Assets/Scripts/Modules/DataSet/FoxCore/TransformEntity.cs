using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public class TransformEntity : DataElement<TransformData>
    {
        public Vector3 Translation;        
        public Vector3 Rotation;
        public Vector3 Scale;
    }
}