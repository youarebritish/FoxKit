using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    public partial class Locator : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Rigidbody)).image as Texture2D;
    }
}
