using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox
{
    public partial class GeoxGeomSetLoader : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(TerrainCollider)).image as Texture2D;
    }
}
