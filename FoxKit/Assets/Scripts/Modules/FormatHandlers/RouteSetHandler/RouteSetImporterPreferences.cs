namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using FoxKit.Utils;

    using UnityEditor;

    using UnityEngine;

    public class RouteSetImporterPreferences : SingletonScriptableObject<RouteSetImporterPreferences>
    {
        private static readonly Color defaultNodeColor = new Color(0.956f, 0.62f, 0.031f);
        private static readonly Color defaultEdgeColor = new Color(0.953f, 0.859f, 0.243f);
        private static readonly float defaultNodeSize = 0.1f;
        
        public Color NodeColor { get; set; } = defaultNodeColor;
        public Color EdgeColor { get; set; } = defaultEdgeColor;
        public float NodeSize { get; set; } = defaultNodeSize;

        public TextAsset IdDictionary { get; set; }
        public TextAsset EventDictionary { get; set; }
        public TextAsset MessageDictionary { get; set; }

        public TextAsset IdHashDump { get; set; }
        public TextAsset EventHashDump { get; set; }
        public TextAsset MessageHashDump { get; set; }
        public TextAsset JsonDump { get; set; }

        [MenuItem("Assets/Create/FoxKit/Preferences/RouteSet")]
        public static void CreateAsset()
        {
            CreateScriptableObject.CreateAsset<RouteSetImporterPreferences>();
        }
    }
}