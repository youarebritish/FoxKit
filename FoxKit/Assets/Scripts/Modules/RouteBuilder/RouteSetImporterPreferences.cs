﻿namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using FoxKit.Utils;

    using UnityEditor;

    using UnityEngine;

    public class RouteSetImporterPreferences : SingletonScriptableObject<RouteSetImporterPreferences>
    {
        private static readonly Color defaultNodeColor = new Color(0.956f, 0.62f, 0.031f);
        private static readonly Color defaultEdgeColor = new Color(0.953f, 0.859f, 0.243f);
        private static readonly float defaultNodeSize = 0.1f;
        
        public Color NodeColor = defaultNodeColor;
        public Color EdgeColor = defaultEdgeColor;
        public float NodeSize = defaultNodeSize;

        public TextAsset IdDictionary;
        public TextAsset EventDictionary;
        public TextAsset MessageDictionary;

        [MenuItem("Assets/Create/FoxKit/Preferences/Route Builder")]
        public static void CreateAsset()
        {
            CreateScriptableObject.CreateAsset<RouteSetImporterPreferences>();
        }
    }
}