namespace FoxKit.Modules.Terrain
{
    using FoxKit.Utils;
    
    using UnityEngine;

    /// <summary>
    /// Terrain preferences.
    /// </summary>
    [CreateAssetMenu(menuName = "FoxKit/Preferences/Terrain Preferences", order = 5)]
    public class TerrainPreferences : SingletonScriptableObject<TerrainPreferences>
    {
        /// <summary>
        /// Default maximum height value.
        /// </summary>
        private static readonly float defaultMaxHeight = MAX_HEIGHT_AFGH;

        /// <summary>
        /// Maximum height value for a terrain.
        /// </summary>
        public float MaxHeight = defaultMaxHeight;

        public const float MAX_HEIGHT_AFGH = 755.0121f;
        public const float MAX_HEIGHT_MAFR = 234.2403f;
        public const float MAX_HEIGHT_CUBA = 80.1147f;
        public const float MAX_HEIGHT_AFDA = 90.19389f;
        public const float MAX_HEIGHT_RMA0 = 755.0121f;
        public const float MAX_HEIGHT_AFN0 = 200f;
        public const float MAX_HEIGHT_AFC0 = 200f;
        public const float MAX_HEIGHT_AFC1 = 200f;
        public const float MAX_HEIGHT_SVA0 = 234.2403f;
    }
}