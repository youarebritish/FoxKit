using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace FoxKit.Modules.DataSet.Fox.Navx
{
    public partial class NavxNavFilter : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(NavMeshData)).image as Texture2D;
    }
}
