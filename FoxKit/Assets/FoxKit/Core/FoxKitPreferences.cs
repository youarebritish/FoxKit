namespace FoxKit
{
    using UnityEngine;
    using FoxKit.Utils;

    /// <summary>
    /// FoxKit general preferences.
    /// </summary>
    [CreateAssetMenu(menuName = "FoxKit/Preferences/General Preferences", order = 6)]
    public class FoxKitPreferences : SingletonScriptableObject<FoxKitPreferences>
    {
        /// <summary>
        /// Path to MGSV:TPP executable.
        /// </summary>
        public string TPPPath = "";

        /// <summary>
        /// Path to SnakeBite folder.
        /// </summary>
        public string SnakeBitePath = "";

        /// <summary>
        /// Path to MakeBite folder.
        /// </summary>
        public string MakeBitePath = "";
    }
}