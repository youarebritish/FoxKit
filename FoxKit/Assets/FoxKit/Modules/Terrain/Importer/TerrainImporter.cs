namespace FoxKit.Modules.Terrain.Importer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;

    /// <summary>
    /// ScriptedImporter to handle importing tre2 files.
    /// </summary>
    [ScriptedImporter(1, "tre2")]
    public class TerrainImporter : ScriptedImporter
    {
        const int HEIGHTMAP_OFFSET_VERSION4 = 672;
        const int HEIGHTMAP_OFFSET_VERSION3 = 640;

        const int HEIGHTMAP_WIDTH = 64;
        const int HEIGHTMAP_HEIGHT = 64;

        const int HEIGHTMAP_BYTES = 4096;

        private const long WidthOffset = 92;

        private const long HeightOffset = 108;

        private const long HighPerLowOffset = 124;

        private const long MaxLodOffset = 140;

        private const long GridDistanceOffset = 156;

        private const long HeightFormatOffset = 220;

        private const long HeightRangeMaxOffset = 236;

        private const long HeightRangeMinOffset = 252;

        private const long DistanceHeightmapWidthOffset = 676;

        private const long DistanceHeightmapHeightOffset = 680;

        private const long HeightmapsOffset = 704;

        /// <summary>
        /// Import a .htre file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var asset = ScriptableObject.CreateInstance<TerrainAsset>();
            asset.name = Path.GetFileNameWithoutExtension(ctx.assetPath);

            ctx.AddObjectToAsset("Main", asset);
            ctx.SetMainObject(asset);

            var heightTiles = new List<float[,]>(4);
            var materialWeightMapTiles = new List<Color[,]>(4);
            var materialIdMapTiles = new List<Color[,]>(4);
            var materialSelectMapTiles = new List<Color[,]>(4);

            const int HalfWidth = HEIGHTMAP_WIDTH / 2;

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                var version = reader.ReadUInt32();

                if (version == 4)
                {
                    reader.BaseStream.Seek(WidthOffset, SeekOrigin.Begin);
                    asset.Width = reader.ReadUInt32();

                    reader.BaseStream.Seek(HeightOffset, SeekOrigin.Begin);
                    asset.Height = reader.ReadUInt32();

                    reader.BaseStream.Seek(HighPerLowOffset, SeekOrigin.Begin);
                    asset.HighPerLow = reader.ReadUInt32();

                    reader.BaseStream.Seek(MaxLodOffset, SeekOrigin.Begin);
                    asset.MaxLod = reader.ReadUInt32();

                    reader.BaseStream.Seek(GridDistanceOffset, SeekOrigin.Begin);
                    asset.GridDistance = reader.ReadSingle();

                    reader.BaseStream.Seek(HeightFormatOffset, SeekOrigin.Begin);
                    asset.HeightFormat = reader.ReadUInt32();

                    reader.BaseStream.Seek(HeightFormatOffset, SeekOrigin.Begin);
                    asset.HeightFormat = reader.ReadUInt32();

                    reader.BaseStream.Seek(HeightRangeMaxOffset, SeekOrigin.Begin);
                    asset.HeightRangeMax = reader.ReadSingle();

                    reader.BaseStream.Seek(HeightRangeMinOffset, SeekOrigin.Begin);
                    asset.HeightRangeMin = reader.ReadSingle();

                    reader.BaseStream.Seek(DistanceHeightmapWidthOffset, SeekOrigin.Begin);
                    asset.DistanceHeightmapWidth = reader.ReadUInt32();

                    reader.BaseStream.Seek(DistanceHeightmapHeightOffset, SeekOrigin.Begin);
                    asset.DistanceHeightmapHeight = reader.ReadUInt32();
                }
                else
                {
                    Debug.LogError("Unrecognized tre2 version number: " + version);
                    return;
                }

                // Read heightmaps.
                reader.BaseStream.Seek(HeightmapsOffset, SeekOrigin.Begin);

                // FIXME
                // The first few are not heightmaps; the 5th one onwards are, though
                for (var i = 0; i < 7; i++)
                {
                    var resolution = asset.DistanceHeightmapHeight / 2;
                    if (i >= 6)
                    {
                        resolution *= 2;
                    }

                    var heights = new float[resolution, resolution];

                    for (var x = 0; x < resolution; x++)
                    {
                        for (var y = 0; y < resolution; y++)
                        {
                            var val = reader.ReadSingle();

                            // TODO Figure out wtf the first few heightmaps are
                            if (i >= 4)
                            {
                                val = (val - asset.HeightRangeMin) / (asset.HeightRangeMax - asset.HeightRangeMin);
                            }

                            heights[y, x] = val;
                        }
                    }

                    var heightmap =
                        new Texture2D((int)resolution, (int)resolution, TextureFormat.RFloat, true)
                            {
                                name =
                                    $"Heightmap {i}"
                            };
                    heightmap.SetPixels((from height in heights.Cast<float>()
                                        select new Color(height, 0, 0)).ToArray());
                    heightmap.Apply(true);
                    asset.DistanceHeightmaps.Add(heightmap);

                    ctx.AddObjectToAsset(heightmap.name, heightmap);
                }

                // Read combo texture.
                {
                    var resolution = 256;
                    var vals = new Color32[resolution, resolution];

                    for (var x = 0; x < resolution; x++)
                    {
                        for (var y = 0; y < resolution; y++)
                        {
                            var r = reader.ReadByte();
                            var g = reader.ReadByte();
                            var b = reader.ReadByte();
                            var a = reader.ReadByte();

                            vals[y, x] = new Color32(r, g, b, a);
                        }
                    }

                    var combo =
                        new Texture2D((int)resolution, (int)resolution, TextureFormat.RGBA32, true)
                            {
                                name = "ComboTexture"
                            };
                    combo.SetPixels32((from height in vals.Cast<Color32>()
                                         select height).ToArray());
                    combo.Apply(true);
                    asset.ComboTexture = combo;

                    ctx.AddObjectToAsset(combo.name, combo);
                }

                // Read material IDs.
                {
                    var resolution = 128;
                    var vals = new Color32[resolution, resolution];

                    for (var x = 0; x < resolution; x++)
                    {
                        for (var y = 0; y < resolution; y++)
                        {
                            var r = reader.ReadByte();
                            var g = reader.ReadByte();
                            var b = reader.ReadByte();
                            var a = reader.ReadByte();

                            vals[y, x] = new Color32(r, g, b, a);
                        }
                    }

                    var ids =
                        new Texture2D((int)resolution, (int)resolution, TextureFormat.RGBA32, true)
                            {
                                name = "Material IDs"
                            };
                    ids.SetPixels32((from height in vals.Cast<Color32>()
                                       select height).ToArray());
                    ids.Apply(true);
                    asset.MaterialIds = ids;

                    ctx.AddObjectToAsset(ids.name, ids);
                }

                // Read configration IDs.
                {
                    var resolution = 128;
                    var vals = new Color32[resolution, resolution];

                    for (var x = 0; x < resolution; x++)
                    {
                        for (var y = 0; y < resolution; y++)
                        {
                            var r = reader.ReadByte();
                            var g = reader.ReadByte();
                            var b = reader.ReadByte();
                            var a = reader.ReadByte();

                            vals[y, x] = new Color32(r, g, b, a);
                        }
                    }

                    var configration =
                        new Texture2D((int)resolution, (int)resolution, TextureFormat.RGBA32, true)
                            {
                                name = "Configration IDs"
                            };
                    configration.SetPixels32((from height in vals.Cast<Color32>()
                                     select height).ToArray());
                    configration.Apply(true);
                    asset.ConfigrationIds = configration;

                    ctx.AddObjectToAsset(configration.name, configration);
                }
            }
        }
    }

}