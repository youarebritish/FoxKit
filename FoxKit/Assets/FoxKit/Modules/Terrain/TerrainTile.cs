using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FoxKit.Modules.Terrain
{
    public class TerrainTile : MonoBehaviour
    {
        public string Level = "afgh";

        [Tooltip("Folder from which to load neighbors. Must be a subfolder of Resources and end in /.")]
        public string Folder = "Terrain/";
        public int IndexX = 133;
        public int IndexZ = 133;

        const int MAX_INDEX = 164;
        const int MIN_INDEX = 101;
        
        public void LoadNeighbors()
        {
            var neighborNames = GetNeighborNames();
            var tilesInScene = from tile in FindObjectsOfType<TerrainTile>()
                                   where tile.Level == Level
                                   select tile.name;

            var neighbors = from name in neighborNames
                            where !tilesInScene.Contains(name)
                            select Resources.Load(string.Concat(Folder, name), typeof(TerrainTile)) as TerrainTile;

            var instances = from neighbor in neighbors
                            where neighbor != null
                            select Instantiate(neighbor);

            foreach(var neighbor in instances)
            {
                neighbor.name = MakeTileName(neighbor.Level, neighbor.IndexX, neighbor.IndexZ);
                neighbor.Folder = Folder;
            }
        }
        
        /// <summary>
        /// Get the names of neighboring tiles.
        /// </summary>
        /// <returns>Names of the neighboring tiles.</returns>
        private IEnumerable<string> GetNeighborNames()
        {
            // Get valid X indices
            var xIndices = new List<int>
            {
                IndexX
            };

            if (IndexX + 1 <= MAX_INDEX)
            {
                xIndices.Add(IndexX + 1);
            }
            if (IndexX - 1 >= MIN_INDEX)
            {
                xIndices.Add(IndexX - 1);
            }

            // Get valid Z indices
            var zIndices = new List<int>
            {
                IndexZ
            };

            if (IndexZ + 1 <= MAX_INDEX)
            {
                zIndices.Add(IndexZ + 1);
            }
            if (IndexZ - 1 >= MIN_INDEX)
            {
                zIndices.Add(IndexZ - 1);
            }
            
            return from indexX in xIndices
                   from indexZ in zIndices
                   select MakeTileName(Level, indexX, indexZ);
        }

        /// <summary>
        /// Get the name of a terrain tile GameObject.
        /// </summary>
        /// <param name="level">Level code of the terrain.</param>
        /// <param name="xIndex">X index of the tile.</param>
        /// <param name="zIndex">Z index of the tile.</param>
        /// <returns>The tile's name.</returns>
        private static string MakeTileName(string level, int xIndex, int zIndex)
        {
            return $"{level}_{xIndex}_{zIndex}_terrain";
        }
    }
}