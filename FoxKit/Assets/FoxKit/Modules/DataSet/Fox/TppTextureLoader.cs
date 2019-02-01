using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox
{
    public partial class TppTextureLoader : Data
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Texture)).image as Texture2D;
    }
}
