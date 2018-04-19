namespace FoxKit.Modules.RouteBuilder
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;
    
    /// <summary>
    /// An AI agent assigned to a Route will walk from node to node in a circuit.
    /// </summary>
    [System.Serializable]
    public class Route : MonoBehaviour
    {
        /// <summary>
        /// Ordered list of route nodes for AI agents to follow.
        /// </summary>
        public List<RouteNode> Nodes = new List<RouteNode>();

        /// <summary>
        /// Should the Route be drawn with the first and last nodes connected?
        /// </summary>
        public bool Closed = true;

        /// <summary>
        /// Should the route's name be exported as a hash instead of a string literal?
        /// </summary>
        public bool TreatNameAsHash;

        /// <summary>
        /// Context menu to add a new node to the Route.
        /// </summary>
        public void AddNewNode()
        {
            CreateRouteSetEditor.CreateNewNode(this);
        }

        /// <summary>
        /// Draw the Route's gizmos in the scene view.
        /// </summary>
        void OnDrawGizmos()
        {
            var isRouteSelected = IsRouteSelected();

            Gizmos.color = RouteBuilderPreferences.Instance.EdgeColor;
            RouteNode previousNode = null;
            foreach (var node in this.Nodes)
            {
                Gizmos.color = RouteBuilderPreferences.Instance.NodeColor;
                if (isRouteSelected)
                {
                    Gizmos.DrawIcon(node.transform.position, "Route Builder/routebuilder_gizmo_node.png", true);
                }
                else
                {
                    Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, Gizmos.color.a * 0.1f);
                }                

                if (previousNode == null)
                {
                    previousNode = node;
                    continue;
                }

                Gizmos.color = RouteBuilderPreferences.Instance.EdgeColor;
                if (!isRouteSelected)
                {
                    Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, Gizmos.color.a * 0.1f);
                }

                Gizmos.DrawLine(previousNode.transform.position, node.transform.position);
                previousNode = node;
            }

            // Connect first and last nodes.
            if (!this.Closed)
            {
                return;
            }
            if (this.Nodes.Count > 2)
            {
                Gizmos.DrawLine(this.Nodes[this.Nodes.Count - 1].transform.position, this.Nodes[0].transform.position);
            }
        }

        /// <summary>
        /// Should the Route be considered selected?
        /// </summary>
        /// <returns>True if selected, else false.</returns>
        private bool IsRouteSelected()
        {
            if (Selection.Contains(this.gameObject))
            {
                return true;
            }
            if (Selection.Contains(this.transform.parent.gameObject))
            {
                return true;
            }
            return Nodes.Any(node => Selection.Contains(node.gameObject));
        }
    }
}