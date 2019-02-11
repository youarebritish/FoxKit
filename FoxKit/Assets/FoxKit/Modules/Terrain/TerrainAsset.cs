namespace FoxKit.Modules.Terrain
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// Terrain configuration asset for a location. Contains a low-resolution terrain to show in the distance.
    /// </summary>
    public class TerrainAsset : ScriptableObject
    {
        /// <summary>
        /// Width of the high-res terrain heightmap in pixels.
        /// </summary>
        [SerializeField]
        private float width;

        /// <summary>
        /// Height of the high-res terrain heightmap in pixels.
        /// </summary>
        [SerializeField]
        private float height;

        [SerializeField]
        private uint highPerLow;

        [SerializeField]
        private uint maxLod;

        /// <summary>
        /// Width of each heightmap pixel in meters.
        /// </summary>
        [SerializeField]
        private float gridDistance;

        [SerializeField]
        private uint heightFormat;

        /// <summary>
        /// Maximum height value in meters.
        /// </summary>
        [SerializeField]
        private float heightRangeMax;

        /// <summary>
        /// Minimum height value in meters.
        /// </summary>
        [SerializeField]
        private float heightRangeMin;

        /// <summary>
        /// Width of the low-res heightmap in pixels.
        /// </summary>
        [SerializeField]
        private uint distanceHeightmapWidth;

        /// <summary>
        /// Height of the low-res heightmap in pixels.
        /// </summary>
        [SerializeField]
        private uint distanceHeightmapHeight;

        [SerializeField]
        private List<Texture2D> distanceHeightmaps = new List<Texture2D>();

        [SerializeField]
        private Texture2D comboTexture;

        [SerializeField]
        private Texture2D materialIds;

        [SerializeField]
        private Texture2D configrationIds;
                
        public float Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        public float Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public uint HighPerLow
        {
            get
            {
                return this.highPerLow;
            }
            set
            {
                this.highPerLow = value;
            }
        }

        public uint MaxLod
        {
            get
            {
                return this.maxLod;
            }
            set
            {
                this.maxLod = value;
            }
        }

        public float GridDistance
        {
            get
            {
                return this.gridDistance;
            }
            set
            {
                this.gridDistance = value;
            }
        }

        public uint HeightFormat
        {
            get
            {
                return this.heightFormat;
            }
            set
            {
                this.heightFormat = value;
            }
        }

        public float HeightRangeMax
        {
            get
            {
                return this.heightRangeMax;
            }
            set
            {
                this.heightRangeMax = value;
            }
        }

        public float HeightRangeMin
        {
            get
            {
                return this.heightRangeMin;
            }
            set
            {
                this.heightRangeMin = value;
            }
        }

        public uint DistanceHeightmapWidth
        {
            get
            {
                return this.distanceHeightmapWidth;
            }
            set
            {
                this.distanceHeightmapWidth = value;
            }
        }

        public uint DistanceHeightmapHeight
        {
            get
            {
                return this.distanceHeightmapHeight;
            }
            set
            {
                this.distanceHeightmapHeight = value;
            }
        }

        public List<Texture2D> DistanceHeightmaps
        {
            get
            {
                return this.distanceHeightmaps;
            }
            set
            {
                this.distanceHeightmaps = value;
            }
        }

        public Texture2D ComboTexture
        {
            get
            {
                return this.comboTexture;
            }
            set
            {
                this.comboTexture = value;
            }
        }

        public Texture2D MaterialIds
        {
            get
            {
                return this.materialIds;
            }
            set
            {
                this.materialIds = value;
            }
        }

        public Texture2D ConfigrationIds
        {
            get
            {
                return this.configrationIds;
            }
            set
            {
                this.configrationIds = value;
            }
        }
    }
}