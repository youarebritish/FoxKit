namespace FoxKit.Modules.RouteBuilder
{
    using FoxKit.Utils;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Editor menu item to create a new RouteSet.
    /// </summary>
    public static class CreateRouteSetEditor
    {
        /// <summary>
        /// Default name for a new RouteSet.
        /// </summary>
        private const string ROUTESET_DEFAULT_NAME = "RouteSet";

        /// <summary>
        /// Default name for a new edge event container.
        /// </summary>
        private const string EDGE_EVENTS_CONTAINER_DEFAULT_NAME = "Edge Events";

        /// <summary>
        /// Create a new RouteSet.
        /// </summary>
        /// <returns>The newly-created RouteSet.</returns>
        [MenuItem("GameObject/FoxKit/RouteSet %#r", false, 0)]
        private static RouteSet CreateRouteSet()
        {
            // Create RouteSet GameObject and component.
            var go = new GameObject();
            var routeSet = go.AddComponent<RouteSet>();
            go.transform.position = Vector3.zero;
            
            go.name = ROUTESET_DEFAULT_NAME;

            // Select new RouteSet.
            UnitySceneUtils.Select(go);

            return routeSet;
        }

        /// <summary>
        /// Create a new RouteNode.
        /// </summary>
        /// <param name="route">Route to own the node.</param>
        public static RouteNode CreateNewNode(Route route)
        {
            var go = new GameObject();

            RouteNode prevNode = null;
            if (route.Nodes.Count > 0)
            {
                prevNode = route.Nodes.Last();
            }
            go.transform.position = GenerateNewNodePosition(prevNode);
            go.transform.SetParent(route.transform);
            go.name = GenerateNewNodeName(route.gameObject.name, route.Nodes.Count);

            var node = go.AddComponent<RouteNode>();
            node.EdgeEvent.Type = route.GetComponentInParent<RouteSet>().DefaultEdgeEventType;
            route.Nodes.Add(node);

            UnitySceneUtils.Select(go);

            return node;
        }

        /// <summary>
        /// Create a new RouteEvent.
        /// </summary>
        /// <param name="parent">GameObject parent to own the event.</param>
        public static RouteEvent CreateNewNodeEvent(GameObject parent)
        {
            var go = new GameObject();
            go.transform.position = parent.transform.position;
            go.transform.SetParent(parent.transform);
            go.name = "RouteEvent";

            UnitySceneUtils.Select(go);

            var routeEvent = go.AddComponent<RouteEvent>();
            return routeEvent;
        }

        /// <summary>
        /// Generate a position for a new node.
        /// </summary>
        /// <remarks>Once 2018.1 is out, change this to be in the center of the scene view.</remarks>
        /// <param name="previousNode">Previous node in the Route, or null if none.</param>
        /// <returns>The new node position.</returns>
        private static Vector3 GenerateNewNodePosition(RouteNode previousNode)
        {
            if (previousNode == null)
            {
                return Vector3.zero;
            }
            return previousNode.transform.position;
        }

        /// <summary>
        /// Generate name for a new node.
        /// </summary>
        /// <param name="eventCount">Number of nodes already in the Route.</param>
        /// <returns>Name for a new node.</returns>
        private static string GenerateNewNodeName(string routeName, int nodeCount)
        {
            return string.Format("{0}_Node{1}", routeName, nodeCount.ToString("D4"));
        }
    }
}