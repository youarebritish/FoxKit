using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
{
    public partial class StaticModelArrayLocator : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Rigidbody)).image as Texture2D;
    }
}
