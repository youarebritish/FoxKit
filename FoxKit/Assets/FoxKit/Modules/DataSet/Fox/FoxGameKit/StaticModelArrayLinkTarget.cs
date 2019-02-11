using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
{
    public partial class StaticModelArrayLinkTarget : Data
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(FixedJoint)).image as Texture2D;
    }
}
