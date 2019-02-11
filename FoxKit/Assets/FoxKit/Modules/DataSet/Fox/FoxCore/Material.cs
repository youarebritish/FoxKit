using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    public partial class Material : Data
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Texture)).image as Texture2D;
    }
}
