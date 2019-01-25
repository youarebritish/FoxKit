namespace FoxKit.Modules.Terrain.Editor
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom editor for Route Builder preferences assets.
    /// </summary>
    public class TerrainPreferencesEditor : EditorWindow
    {
        private int presetIndex;

        /// <summary>
        /// Create a preferences window.
        /// </summary>
        [MenuItem("FoxKit/Preferences/Terrain")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TerrainPreferencesEditor), true, "Terrain Preferences");
        }        

        void OnGUI()
        {
            var prefs = TerrainPreferences.Instance;
            
            prefs.TerrainShader = (Shader)EditorGUILayout.ObjectField("Shader", prefs.TerrainShader, typeof(Shader), false);
        }
    }
}