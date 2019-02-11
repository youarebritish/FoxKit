using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
{
    public partial class TerrainDecalPack : Data
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Tilemap)).image as Texture2D;
    }
}
