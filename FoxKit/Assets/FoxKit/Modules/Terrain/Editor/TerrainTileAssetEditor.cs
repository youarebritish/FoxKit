using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.Terrain.Editor
{
    using System;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TerrainTileAsset))]
    public class TerrainTileAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUI.enabled = true;
            if (GUILayout.Button("Export heightmap"))
            {
                var path = EditorUtility.SaveFilePanelInProject(
                    "Export heightmap",
                    this.target.name,
                    "asset",
                    string.Empty);

                var heightmap = (this.target as TerrainTileAsset).Heightmap;
                var newTexture = new Texture2D(heightmap.width, heightmap.height, heightmap.format, true);
                var texData = heightmap.GetRawTextureData<float>();
                newTexture.LoadRawTextureData(texData);
                newTexture.Apply();

                AssetDatabase.CreateAsset(newTexture, path);
            }
        }
    }
}