namespace FoxKit.Modules.Terrain.Importer
{
    using System.IO;

    using UnityEditor.Experimental.AssetImporters;
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// ScriptedImporter to handle importing htre files.
    /// </summary>
    [ScriptedImporter(1, "htre")]
    public class TerrainTileImporter : ScriptedImporter
    {
        const int HEIGHTMAP_OFFSET = 672;

        const int HEIGHTMAP_WIDTH = 64;
        const int HEIGHTMAP_HEIGHT = 64;

        const int HEIGHTMAP_BYTES = 4096;

        // TODO: Make parameters
        const float MAX_ELEVATION = 755.0121f;
        const float MIN_ELEVATION = 188.4435f;

        /// <summary>
        /// Import a .frld file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var tiles = new List<float[,]>(4);// (new float[HEIGHTMAP_WIDTH, HEIGHTMAP_HEIGHT];

            float min = float.MaxValue;
            float max = float.MinValue;
            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                SkipBytes(reader, HEIGHTMAP_OFFSET);

                for (int tile = 0; tile < 4; tile++)
                {
                    var heightValues = new float[HEIGHTMAP_WIDTH / 2, HEIGHTMAP_HEIGHT / 2];
                    tiles.Add(heightValues);

                    for (int i = 0; i < HEIGHTMAP_WIDTH / 2; i++)
                    {
                        for (int j = 0; j < HEIGHTMAP_HEIGHT / 2; j++)
                        {
                            var height = reader.ReadSingle();
                            heightValues[i, j] = height / MAX_ELEVATION;

                            if (height > max)
                            {
                                max = height;
                            }
                            else if (height < min)
                            {
                                min = height;
                            }
                        }
                    }
                }
            }
            
            var terrainGO = new GameObject(Path.GetFileNameWithoutExtension(ctx.assetPath));
            var terrainData = new TerrainData
            {
                heightmapResolution = HEIGHTMAP_WIDTH,
                size = new Vector3(128.0f, MAX_ELEVATION, 128.0f)
            };

            terrainData.SetHeights(0, 0, tiles[0]);
            terrainData.SetHeights(HEIGHTMAP_WIDTH / 2, 0, tiles[1]);
            terrainData.SetHeights(0, HEIGHTMAP_WIDTH / 2, tiles[2]);
            terrainData.SetHeights(HEIGHTMAP_WIDTH / 2, HEIGHTMAP_WIDTH / 2, tiles[3]);

            var terrainCollider = terrainGO.AddComponent<TerrainCollider>();
            var terrain = terrainGO.AddComponent<Terrain>();

            terrainCollider.terrainData = terrainData;
            terrain.terrainData = terrainData;

            ctx.AddObjectToAsset(terrainGO.name, terrainGO);
            ctx.AddObjectToAsset(terrainGO.name, terrainData);
            ctx.SetMainObject(terrainGO);
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