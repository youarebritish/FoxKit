namespace FoxKit.Modules.Terrain
{
    using UnityEngine;

    public class TerrainTileAsset : ScriptableObject
    {
        public Texture2D Heightmap;
        public Texture2D MaterialWeightMap;
        public Texture2D MaterialIndicesMap;
        public Texture2D MaterialSelectMap;
    }
}