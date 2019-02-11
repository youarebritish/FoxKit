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
        /// Shader to use for terrain.
        /// </summary>
        public Shader TerrainShader;
    }
}