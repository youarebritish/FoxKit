namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using FoxKit.Utils;

    using UnityEngine;

    /// <summary>
    /// Form Variation Builder preferences.
    /// </summary>
    [CreateAssetMenu(menuName = "FoxKit/Preferences/Form Variation Preferences", order = 5)]
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
    }
}