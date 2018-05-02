namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using FoxKit.Utils;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Form Variation Builder preferences.
    /// </summary>
    public class FormVariationPreferences : SingletonScriptableObject<FormVariationPreferences>
    {
        /// <summary>
        /// Name dictionary.
        /// </summary>
        public TextAsset NameDictionary;

        /// <summary>
        /// File name dictionary.
        /// </summary>
        public TextAsset FileDictionary;

        /// <summary>
        /// Creates a new Form Variation Builder asset.
        /// </summary>
        [MenuItem("Assets/Create/FoxKit/Preferences/Part Builder/Form Variation")]
        public static void CreateAsset()
        {
            CreateScriptableObject.CreateAsset<FormVariationPreferences>();
        }
    }
}