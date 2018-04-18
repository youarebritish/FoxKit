namespace FoxKit.Modules.Terrain.Editor
{
    using UnityEditor;

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
            var maxHeight = prefs.MaxHeight;

            prefs.MaxHeight = EditorGUILayout.FloatField("Maximum height:", prefs.MaxHeight);

            EditorGUILayout.Space();

            var maxHeightPresets = new string[] { "afgh", "mafr", "cuba", "afda", "rma0", "afn0", "afc0", "afc1", "sva0", "Custom" };
            presetIndex = EditorGUILayout.Popup(presetIndex, maxHeightPresets);
            switch (presetIndex)
            {
                case 0:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_AFGH;
                    break;
                case 1:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_MAFR;
                    break;
                case 2:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_CUBA;
                    break;
                case 3:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_AFDA;
                    break;
                case 4:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_RMA0;
                    break;
                case 5:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_AFN0;
                    break;
                case 6:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_AFC0;
                    break;
                case 7:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_AFC1;
                    break;
                case 8:
                    prefs.MaxHeight = TerrainPreferences.MAX_HEIGHT_SVA0;
                    break;
                case 9:
                    break;
            }
        }
    }
}