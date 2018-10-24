namespace FoxKit.Modules.RouteBuilder
{
    using FoxKit.Utils;
    
    using UnityEngine;

    /// <summary>
    /// Route Builder preferences.
    /// </summary>
    [CreateAssetMenu(menuName = "FoxKit/Preferences/Route Builder Preferences", order = 5)]
    public class RouteBuilderPreferences : SingletonScriptableObject<RouteBuilderPreferences>
    {
        /// <summary>
        /// Default widget color for route nodes.
        /// </summary>
        private static readonly Color defaultNodeColor = new Color(0.956f, 0.62f, 0.031f);

        /// <summary>
        /// Default widget color for lines connecting route nodes.
        /// </summary>
        private static readonly Color defaultEdgeColor = new Color(0.953f, 0.859f, 0.243f);

        /// <summary>
        /// Default widget size for route nodes.
        /// </summary>
        private static readonly float defaultNodeSize = 0.1f;
        
        /// <summary>
        /// Widget color for route nodes.
        /// </summary>
        public Color NodeColor = defaultNodeColor;

        /// <summary>
        /// Widget color for lines connecting route nodes.
        /// </summary>
        public Color EdgeColor = defaultEdgeColor;

        /// <summary>
        /// Widget size for route nodes.
        /// </summary>
        public float NodeSize = defaultNodeSize;

        /// <summary>
        /// Route ID dictionary.
        /// </summary>
        public TextAsset IdDictionary;

        /// <summary>
        /// Route event type dictionary.
        /// </summary>
        public TextAsset EventDictionary;

        /// <summary>
        /// Route event message dictionary.
        /// </summary>
        public TextAsset MessageDictionary;
    }
}