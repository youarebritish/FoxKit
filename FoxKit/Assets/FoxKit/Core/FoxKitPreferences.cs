namespace FoxKit
{
    using UnityEngine;
    using FoxKit.Utils;

    /// <summary>
    /// Route Builder preferences.
    /// </summary>
    [CreateAssetMenu(menuName = "FoxKit/Preferences/General Preferences", order = 6)]
    public class FoxKitPreferences : SingletonScriptableObject<FoxKitPreferences>
    {
        /// <summary>
        /// Path to MGSV:TPP executable.
        /// </summary>
        public string TPPPath = "";

        /// <summary>
        /// Path to SnakeBite executable.
        /// </summary>
        public string SnakeBitePath = "";
    }
}