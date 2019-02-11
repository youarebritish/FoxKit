using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
{
    public partial class ObjectBrushPlugin : Data
    {
        public override Texture2D Icon => EditorGUIUtility.FindTexture("d_tree_icon_frond");
    }
}
