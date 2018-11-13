using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Modules.RouteBuilder;
    using System;
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Custom editor for Route Builder preferences assets.
    /// </summary>
    public class FoxKitPrefrencesEditor : EditorWindow
    {
        /// <summary>
        /// Create a preferences window for Route Builder.
        /// </summary>
        [MenuItem("FoxKit/Preferences/General")]
        public static void ShowWindow()
        {
            GetWindow(typeof(FoxKitPrefrencesEditor), true, "FoxKit Preferences");
        }

        void OnGUI()
        {
            var prefs = FoxKitPreferences.Instance;

            EditorGUILayout.LabelField("Gizmos", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("TPP .exe Path");
                prefs.TPPPath = EditorGUILayout.TextField(prefs.TPPPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Snakebite .exe path");
                prefs.SnakeBitePath = EditorGUILayout.TextField(prefs.SnakeBitePath);
            EditorGUILayout.EndHorizontal();
        }
    }
}