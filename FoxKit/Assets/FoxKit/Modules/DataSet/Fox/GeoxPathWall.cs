using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace FoxKit.Modules.DataSet.Fox
{
    public enum Face_Mode : int
    {
        CW = 0,
        CCW = 1,
        Double = 2
    }

    public partial class GeoxPathWall
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(NavMeshObstacle)).image as Texture2D;
    }
}
