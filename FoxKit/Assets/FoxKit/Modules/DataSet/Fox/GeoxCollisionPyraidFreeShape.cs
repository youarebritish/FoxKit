using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox
{
    public enum CollisionPyraidFreeShape_Category : int
    {
        Alll = 0,
        Chara = 1,
        Recoil = 2
    }

    public partial class GeoxCollisionPyraidFreeShape
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MeshCollider)).image as Texture2D;
    }
}
