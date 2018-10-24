using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.Terrain.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TerrainTile))]
    public class TerrainTileEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Load neighbors"))
            {
                var tile = (TerrainTile)target;
                tile.LoadNeighbors();
            }
        }
    }
}