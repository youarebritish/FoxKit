using UnityEditor;

namespace FoxKit.Modules.Terrain.Editor
{
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Utils;

    using UnityEngine;
    using UnityEngine.Assertions;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TerrainAsset))]
    public class TerrainAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = true;
            
            var asset = this.target as TerrainAsset;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Stitch terrain tiles"))
            {
                var path = EditorUtility.SaveFilePanelInProject(
                    "Save terrain atlas",
                    asset.name + "_atlas",
                    "asset",
                    "Enter a file name to save the terrain atlas to.");
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }

                var tiles = this.GetTerrainTiles();
                var atlas = StitchTerrainTiles((int)(asset.Width - 1), (int)(asset.Height - 1), tiles);

                AssetDatabase.CreateAsset(atlas, path);
                AssetDatabase.Refresh();
            }
            else if (GUILayout.Button("Add to scene"))
            {
                var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.position = Vector3.zero;
                plane.transform.localScale = new Vector3(asset.Width * asset.GridDistance / 10.0f, 1.0f, asset.Height * asset.GridDistance / 10.0f);
                plane.name = asset.name;

                var material = new Material(TerrainPreferences.Instance.TerrainShader);
                material.SetFloat("_ParallaxStrengthMin", asset.HeightRangeMin);
                material.SetFloat("_ParallaxStrengthMax", asset.HeightRangeMax);
                // TODO Set heightmap

                plane.GetComponent<Renderer>().material = material;
            }

            EditorGUILayout.EndHorizontal();

            this.DrawDefaultInspector();
        }

        private static Texture2D StitchTerrainTiles(int atlasWidth, int atlasHeight, IEnumerable<TerrainTileAsset> tiles)
        {
            var atlas = new Texture2D(atlasWidth, atlasHeight, TextureFormat.RGBAFloat, false);
            var heightmaps = from tile in tiles
                             select tile.Heightmap;

            foreach (var heightmap in heightmaps)
            {
                var xIndex = int.Parse(heightmap.name.Substring(5, 3)) - 101;
                var yIndex = int.Parse(heightmap.name.Substring(9, 3)) - 101;

                // FIXME
                // Currently, htre heightmaps come in rotated, so unrotate them.
                var heightmapPixels = heightmap.GetPixels();
                var rotatedHeightmap = new Color[heightmap.width * heightmap.height];
                for (var i = 0; i < heightmap.width; ++i)
                {
                    for (var j = 0; j < heightmap.height; ++j)
                    {
                        rotatedHeightmap[i * heightmap.width + j] = heightmapPixels[(heightmap.width - j - 1) * heightmap.width + i];
                    }
                }

                atlas.SetPixels(
                    atlasWidth - ((xIndex + 1) * heightmap.width),
                    yIndex * heightmap.height,
                    heightmap.width,
                    heightmap.height,
                    rotatedHeightmap);
            }

            // Not sure why but it winds up flipped horizontally and vertically. Let's fix that.
            var atlasFlippedHorizontal = new Texture2D(atlasWidth, atlasHeight, TextureFormat.RGBAFloat, false);
            var xN = atlasFlippedHorizontal.width;
            var yN = atlasFlippedHorizontal.height;
            
            for (var i = 0; i < xN; i++)
            {
                for (var j = 0; j < yN; j++)
                {
                    atlasFlippedHorizontal.SetPixel(j, xN - i - 1, atlas.GetPixel(j, i));
                }
            }

            atlasFlippedHorizontal.Apply();

            var atlasFlippedVertical = new Texture2D(atlasWidth, atlasHeight, TextureFormat.RGBAFloat, false);
            for (var i = 0; i < xN; i++)
            {
                for (var j = 0; j < yN; j++)
                {
                    atlasFlippedVertical.SetPixel(xN - i - 1, j, atlasFlippedHorizontal.GetPixel(i, j));
                }
            }

            atlasFlippedVertical.Apply();
            return atlasFlippedVertical;
        }

        private IEnumerable<TerrainTileAsset> GetTerrainTiles()
        {
            var asset = this.target as TerrainAsset;
            Assert.IsNotNull(asset);
            
            var baseName = asset.name.Substring(0, 4);
            return from tile in UnityFileUtils.GetAllAssetsOfType<TerrainTileAsset>()
                   where tile.name.StartsWith(baseName)
                   select tile;
        }
    }
}