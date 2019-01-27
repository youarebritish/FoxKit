using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoxKit.Core.Editor
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

            EditorGUILayout.LabelField("Executables", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("TPP");
            prefs.TPPPath = EditorGUILayout.TextField(prefs.TPPPath);
            if (GUILayout.Button("Select"))
            {
                prefs.TPPPath = EditorUtility.OpenFilePanel("Select TPP .exe path", string.Empty, "exe");
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("SnakeBite");
            prefs.TPPPath = EditorGUILayout.TextField(prefs.TPPPath);
            if (GUILayout.Button("Select"))
            {
                prefs.SnakeBitePath = EditorUtility.OpenFilePanel("Select SnakeBite .exe path", string.Empty, "exe");
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("MakeBite");
            prefs.TPPPath = EditorGUILayout.TextField(prefs.TPPPath);
            if (GUILayout.Button("Select"))
            {
                prefs.MakeBitePath = EditorUtility.OpenFilePanel("Select MakeBite .exe path", string.Empty, "exe");
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}