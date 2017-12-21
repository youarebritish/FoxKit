namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using UnityEngine;

    public class RouteSetImporterPreferences : ScriptableObject
    {
        private static readonly Color defaultNodeColor = new Color(244, 158, 8);
        private static readonly Color defaultEdgeColor = new Color(243, 210, 62);
        private static readonly float defaultNodeSize = 0.1f;

        public Color NodeColor => defaultNodeColor;
        public Color EdgeColor => defaultEdgeColor;
        public float NodeSize => defaultNodeSize;

        public TextAsset IdDictionary { get; set; }
        public TextAsset EventDictionary { get; set; }
        public TextAsset MessageDictionary { get; set; }

        public TextAsset IdHashDump { get; set; }
        public TextAsset EventHashDump { get; set; }
        public TextAsset MessageHashDump { get; set; }
        public TextAsset JsonDump { get; set; }
    }
}