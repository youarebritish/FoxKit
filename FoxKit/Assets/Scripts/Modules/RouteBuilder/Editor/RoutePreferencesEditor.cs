namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using System;
    using UnityEditor;

    using UnityEngine;

    public class RoutePreferencesEditor : EditorWindow
    {
        [MenuItem("FoxKit/Preferences/Route Builder")]
        public static void ShowWindow()
        {
            GetWindow(typeof(RoutePreferencesEditor), true, "Route Builder Preferences");
        }

        void OnGUI()
        {
            var prefs = RouteSetImporterPreferences.Instance;

            EditorGUILayout.LabelField("Gizmos", EditorStyles.boldLabel);
            
            prefs.NodeColor = EditorGUILayout.ColorField("Node Color", prefs.NodeColor);
            prefs.EdgeColor = EditorGUILayout.ColorField("Edge Color", prefs.EdgeColor);
            prefs.NodeSize = EditorGUILayout.Slider("Node Size", prefs.NodeSize, 0.01f, 1.0f);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Dictionaries", EditorStyles.boldLabel);
            prefs.IdDictionary = EditorGUILayout.ObjectField("Route ID", prefs.IdDictionary, typeof(TextAsset), false) as TextAsset;
            prefs.EventDictionary = EditorGUILayout.ObjectField("Event Name", prefs.EventDictionary, typeof(TextAsset), false) as TextAsset;
            prefs.MessageDictionary = EditorGUILayout.ObjectField("Message", prefs.MessageDictionary, typeof(TextAsset), false) as TextAsset;
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Restore Defaults"))
            {
                throw new NotImplementedException();
            }
        }
    }
}