using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox
{
    public partial class GeoxPathPack : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(LineRenderer)).image as Texture2D;
    }
}
