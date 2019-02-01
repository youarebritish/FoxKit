using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.Grx
{
    public partial class OccluderArray : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(OcclusionArea)).image as Texture2D;
    }
}
