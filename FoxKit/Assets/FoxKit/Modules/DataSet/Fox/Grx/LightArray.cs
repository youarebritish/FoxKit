using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.Grx
{    
    public partial class LightArray : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(LightProbeProxyVolume)).image as Texture2D;
    }
}
