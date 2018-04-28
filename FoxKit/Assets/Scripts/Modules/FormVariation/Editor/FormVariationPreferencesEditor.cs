namespace FoxKit.Modules.PartsBuilder.FormVariation.Editor
{
    using System;
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Custom editor for Route Builder preferences assets.
    /// </summary>
    public class FormVariationPreferencesEditor : EditorWindow
    {
        /// <summary>
        /// Create a preferences window for Route Builder.
        /// </summary>
        [MenuItem("FoxKit/Preferences/Parts Builder/Form Variation")]
        public static void ShowWindow()
        {
            GetWindow(typeof(FormVariationPreferencesEditor), true, "Form Variation Editor Preferences");
        }

        void OnGUI()
        {
            var prefs = FormVariationPreferences.Instance;

            EditorGUILayout.LabelField("Dictionaries", EditorStyles.boldLabel);
            prefs.NameDictionary = EditorGUILayout.ObjectField("Names", prefs.NameDictionary, typeof(TextAsset), false) as TextAsset;
            prefs.FileDictionary = EditorGUILayout.ObjectField("Files", prefs.FileDictionary, typeof(TextAsset), false) as TextAsset;
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Restore Defaults"))
            {
                throw new NotImplementedException();
            }
        }
    }
}