namespace FoxKit.Modules.Terrain.Importer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;

    /// <summary>
    /// ScriptedImporter to handle importing htre files. Currently only imports heightmap data.
    /// </summary>
    [ScriptedImporter(1, "htre")]
    public class TerrainTileImporter : ScriptedImporter
    {
        const int HEIGHTMAP_OFFSET_VERSION4 = 672;
        const int HEIGHTMAP_OFFSET_VERSION3 = 640;

        const int HEIGHTMAP_WIDTH = 64;
        const int HEIGHTMAP_HEIGHT = 64;

        const int HEIGHTMAP_BYTES = 4096;

        /// <summary>
        /// Import a .htre file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var heightTiles = new List<float[,]>(4);
            var materialWeightMapTiles = new List<Color[,]>(4);
            var materialIdMapTiles = new List<Color[,]>(4);
            var materialSelectMapTiles = new List<Color[,]>(4);

            const int HalfWidth = HEIGHTMAP_WIDTH / 2;


            var asset = ScriptableObject.CreateInstance<TerrainTileAsset>();
            asset.name = Path.GetFileNameWithoutExtension(ctx.assetPath);

            ctx.AddObjectToAsset("Main", asset);
            ctx.SetMainObject(asset);

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                var version = reader.ReadUInt32();

                if (version == 3)
                {
                    reader.BaseStream.Seek(HEIGHTMAP_OFFSET_VERSION3, SeekOrigin.Begin);
                }
                else if (version == 4)
                {
                    reader.BaseStream.Seek(HEIGHTMAP_OFFSET_VERSION4, SeekOrigin.Begin);
                }
                else
                {
                    Debug.LogError("Unrecognized htre version number: " + version);
                }

                // Read heightmap
                var prefs = TerrainPreferences.Instance;
                for (var tile = 0; tile < 4; tile++)
                {
                    var heightValues = new float[HEIGHTMAP_WIDTH / 2, HEIGHTMAP_HEIGHT / 2];
                    heightTiles.Add(heightValues);

                    for (var i = 0; i < HalfWidth; i++)
                    {
                        for (var j = 0; j < HalfWidth; j++)
                        {
                            var height = reader.ReadSingle();
                            heightValues[j, i] = (height - prefs.MinHeight) / (prefs.MaxHeight - prefs.MinHeight);
                        }
                    }
                }

                // Read material weight map
                for (var tile = 0; tile < 4; tile++)
                {
                    var materialWeightValues = new Color[HEIGHTMAP_WIDTH / 2, HEIGHTMAP_HEIGHT / 2];
                    materialWeightMapTiles.Add(materialWeightValues);

                    for (var i = 0; i < HalfWidth; i++)
                    {
                        for (var j = 0; j < HalfWidth; j++)
                        {
                            var r = reader.ReadByte();
                            var g = reader.ReadByte();
                            var b = reader.ReadByte();
                            var a = reader.ReadByte();
                            materialWeightValues[j, i] = new Color(r, g, b, a);
                        }
                    }
                }

                // Skip unknown section
                reader.BaseStream.Position += 64L;

                // Read material ID map
                // This can't be only 2x2, can it?
                for (var tile = 0; tile < 4; tile++)
                {
                    var materialIdValues = new Color[2, 2];
                    materialIdMapTiles.Add(materialIdValues);

                    for (var i = 0; i < 1; i++)
                    {
                        for (var j = 0; j < 1; j++)
                        {
                            var r = reader.ReadByte();
                            var g = reader.ReadByte();
                            var b = reader.ReadByte();
                            var a = reader.ReadByte();
                            materialIdValues[j, i] = new Color(r, g, b, a);
                        }
                    }
                }

                // Skip unknown (heightmap LOD?) section
                reader.BaseStream.Position += 32L;

                // Read material select map
                // This can't be only 2x2, can it?
                for (var tile = 0; tile < 4; tile++)
                {
                    var materialSelectValues = new Color[2, 2];
                    materialSelectMapTiles.Add(materialSelectValues);

                    for (var i = 0; i < 1; i++)
                    {
                        for (var j = 0; j < 1; j++)
                        {
                            var r = reader.ReadByte();
                            var g = reader.ReadByte();
                            var b = reader.ReadByte();
                            var a = reader.ReadByte();
                            materialSelectValues[j, i] = new Color(r, g, b, a);
                        }
                    }
                }
            }

            // Create terrain asset
            /*var terrainGo = new GameObject(Path.GetFileNameWithoutExtension(ctx.assetPath));
            var terrainData = new TerrainData
            {
                heightmapResolution = HEIGHTMAP_WIDTH,
                size = new Vector3(128.0f, TerrainPreferences.Instance.MaxHeight, 128.0f)
            };            

            terrainData.SetHeights(0, 0, heightTiles[0]);
            terrainData.SetHeights(HalfWidth, 0, heightTiles[2]);

            terrainData.SetHeights(0, HalfWidth, heightTiles[1]);
            terrainData.SetHeights(HalfWidth, HalfWidth, heightTiles[3]);

            var terrainCollider = terrainGo.AddComponent<TerrainCollider>();
            var terrain = terrainGo.AddComponent<Terrain>();

            terrainCollider.terrainData = terrainData;
            terrain.terrainData = terrainData;

            // Parse name and position based on name
            var xIndex = int.Parse(terrainGo.name.Substring(5, 3)) - 101;            
            var zIndex = int.Parse(terrainGo.name.Substring(9, 3)) - 101;

            var tileComponent = terrainGo.AddComponent<TerrainTile>();
            tileComponent.Level = terrainGo.name.Substring(0, 4);
            tileComponent.IndexX = xIndex + 101;
            tileComponent.IndexZ = zIndex + 101;

            terrainGo.transform.position = new Vector3(-4096 + (128 * zIndex), 0, -4096 + (128 * xIndex));

            ctx.AddObjectToAsset(terrainGo.name, terrainGo);
            ctx.AddObjectToAsset(terrainGo.name, terrainData);
            ctx.SetMainObject(terrainGo);*/
            
            var name = Path.GetFileNameWithoutExtension(ctx.assetPath);

            // Create material weight map.
            var materialWeightMap =
                new Texture2D(64, 64, TextureFormat.ARGB32, true) { name = name + "_MaterialWeightMap" };
            materialWeightMap.wrapMode = TextureWrapMode.Clamp;

            materialWeightMap.SetPixels(0, 0, HalfWidth, HalfWidth, materialWeightMapTiles[0].Cast<Color>().ToArray());
            materialWeightMap.SetPixels(HalfWidth, 0, HalfWidth, HalfWidth, materialWeightMapTiles[2].Cast<Color>().ToArray());

            materialWeightMap.SetPixels(0, HalfWidth, HalfWidth, HalfWidth, materialWeightMapTiles[1].Cast<Color>().ToArray());
            materialWeightMap.SetPixels(HalfWidth, HalfWidth, HalfWidth, HalfWidth, materialWeightMapTiles[3].Cast<Color>().ToArray());

            ctx.AddObjectToAsset(name + "MaterialWeightMap", materialWeightMap);

            // Create material ID map.
            var materialIndicesMap =
                new Texture2D(2, 2, TextureFormat.ARGB32, true) { name = name + "_MaterialIndicesMap" };
            materialIndicesMap.wrapMode = TextureWrapMode.Clamp;

            materialIndicesMap.SetPixels(0, 0, 1, 1, materialIdMapTiles[0].Cast<Color>().ToArray());
            materialIndicesMap.SetPixels(1, 0, 1, 1, materialIdMapTiles[2].Cast<Color>().ToArray());

            materialIndicesMap.SetPixels(0, 1, 1, 1, materialIdMapTiles[1].Cast<Color>().ToArray());
            materialIndicesMap.SetPixels(1, 1, 1, 1, materialIdMapTiles[3].Cast<Color>().ToArray());

            ctx.AddObjectToAsset(name + "MaterialIndicesMap", materialIndicesMap);

            // Create material select map.
            var materialSelectMap =
                new Texture2D(2, 2, TextureFormat.ARGB32, true) { name = name + "_MaterialSelectMap" };
            materialSelectMap.wrapMode = TextureWrapMode.Clamp;

            materialSelectMap.SetPixels(0, 0, 1, 1, materialSelectMapTiles[0].Cast<Color>().ToArray());
            materialSelectMap.SetPixels(1, 0, 1, 1, materialSelectMapTiles[2].Cast<Color>().ToArray());

            materialSelectMap.SetPixels(0, 1, 1, 1, materialSelectMapTiles[1].Cast<Color>().ToArray());
            materialSelectMap.SetPixels(1, 1, 1, 1, materialSelectMapTiles[3].Cast<Color>().ToArray());

            ctx.AddObjectToAsset(name + "MaterialSelectMap", materialSelectMap);

            // Create heightmap.
            var heightMap = new Texture2D(HEIGHTMAP_WIDTH, HEIGHTMAP_HEIGHT, TextureFormat.RFloat, true, true);
            heightMap.name = name + "_Heightmap";
            heightMap.wrapMode = TextureWrapMode.Clamp;
            heightMap.filterMode = FilterMode.Bilinear;

            var colors = from height in heightTiles[0].Cast<float>()
                         select new Color(height, height, height);
            heightMap.SetPixels(0, 0, HalfWidth, HalfWidth, colors.ToArray());

            colors = from height in heightTiles[2].Cast<float>()
                     select new Color(height, height, height);
            heightMap.SetPixels(HalfWidth, 0, HalfWidth, HalfWidth, colors.ToArray());

            colors = from height in heightTiles[1].Cast<float>()
                     select new Color(height, height, height);
            heightMap.SetPixels(0, HalfWidth, HalfWidth, HalfWidth, colors.ToArray());

            colors = from height in heightTiles[3].Cast<float>()
                     select new Color(height, height, height);
            heightMap.SetPixels(HalfWidth, HalfWidth, HalfWidth, HalfWidth, colors.ToArray());

            ctx.AddObjectToAsset(name + "HeightMap", heightMap);
            
            asset.Heightmap = heightMap;
            asset.MaterialSelectMap = materialSelectMap;
            asset.MaterialIndicesMap = materialIndicesMap;
            asset.MaterialWeightMap = materialWeightMap;
        }

        /// <summary>
        /// Skip reading a number of bytes.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        /// <param name="numberOfBytes">The number of bytes to skip.</param>
        private static void SkipBytes(BinaryReader reader, int numberOfBytes)
        {
            reader.BaseStream.Position += numberOfBytes;
        }
    }

}