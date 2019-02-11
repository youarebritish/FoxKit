using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    public partial class BoxShape : ShapeData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(BoxCollider)).image as Texture2D;
    }
}
