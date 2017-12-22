using System.IO;

using UnityEditor.Experimental.AssetImporters;

using UnityEngine;

[ScriptedImporter(1, "tre2")]
public class TerrainImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
        {
            reader.BaseStream.Position = 262848;
            
            float maxVal = 755.0121f;
            float minVal = 188.4435f;

            float[,] data = new float[128, 128];
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    float val = reader.ReadSingle();
                    float normalizedVal = (val - minVal) / maxVal;
                    data[x, y] = normalizedVal;
                }
            }

            GameObject TerrainObj = new GameObject(Path.GetFileNameWithoutExtension(ctx.assetPath));
            TerrainObj.transform.position = new Vector3(-4096, 0, -4096);

            TerrainData _TerrainData = new TerrainData
            {
                size = new Vector3(2048, maxVal, 2048),
                heightmapResolution = 128,
                baseMapResolution = 128
            };
            _TerrainData.SetDetailResolution(128, 16);
            _TerrainData.SetHeights(0, 0, data);
            
            TerrainCollider _TerrainCollider = TerrainObj.AddComponent<TerrainCollider>();
            Terrain _Terrain2 = TerrainObj.AddComponent<Terrain>();

            _TerrainCollider.terrainData = _TerrainData;
            _Terrain2.terrainData = _TerrainData;

            ctx.AddObjectToAsset(Path.GetFileNameWithoutExtension(ctx.assetPath), TerrainObj);
            ctx.AddObjectToAsset("TerrainData", _TerrainData);
            ctx.SetMainObject(TerrainObj);

            reader.BaseStream.Position = 704;
            bool isHeightValue = false;
            for (int z = 0; z < 10; z++)
            {
                if (z > 3)
                {
                    isHeightValue = true;
                }
                var heightmap = new Texture2D(128, 128);
                for (int i = 0; i < 16384; i++)
                {
                    float val = reader.ReadSingle();
                    float normalizedVal = (val - minVal) / maxVal;
                    Color color;
                    if (isHeightValue)
                    {
                        color = new Color(normalizedVal, normalizedVal, normalizedVal);
                    }
                    else
                    {
                        float otherMax = 10;
                        color = new Color(val/ otherMax, val/ otherMax, val/ otherMax);
                    }
                    var x = i % 128;
                    var y = i / 128;
                    heightmap.SetPixel(y, x, color);
                }
                ctx.AddObjectToAsset("heightmap" + z, heightmap);
            }
        }
    }
}
