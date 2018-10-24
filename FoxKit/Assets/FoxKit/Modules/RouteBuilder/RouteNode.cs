namespace FoxKit.Modules.RouteBuilder
{
    using FoxKit.Utils;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// A point in a Route for an AI agent to navigate to.
    /// </summary>
    [System.Serializable]
    [RequireComponent(typeof(RouteEdgeEvent))]
    public class RouteNode : MonoBehaviour
    {
        /// <summary>
        /// Event for AI agents to perform on the way to this node.
        /// </summary>
        public RouteEdgeEvent EdgeEvent {  get { return GetComponent<RouteEdgeEvent>(); } }

        /// <summary>
        /// Events for AI agents to perform at this node.
        /// </summary>
        public List<RouteNodeEvent> Events = new List<RouteNodeEvent>();

        /// <summary>
        /// Context menu to add a new node to the Route.
        /// </summary>
        public void AddNewNode()
        {
            CreateRouteSetEditor.CreateNewNode(transform.GetComponentInParent<Route>());
        }

        /// <summary>
        /// Context menu to add a new node event to the node.
        /// </summary>
        public void AddNewEvent()
        {
            var @event = RouteNodeEvent.CreateNewNodeEvent(this, GetComponentInParent<RouteSet>().DefaultNodeEventType);
            this.Events.Add(@event);
        }

        /// <summary>
        /// Select the next node.
        /// </summary>
        public void SelectNextNode()
        {
            var route = transform.GetComponentInParent<Route>();
            var id = route.Nodes.IndexOf(this);

            GameObject nextNode = null;
            if (id >= route.Nodes.Count - 1)
            {
                nextNode = route.Nodes[0].gameObject;
            }
            else
            {
                nextNode = route.Nodes[id + 1].gameObject;
            }
            UnitySceneUtils.Select(nextNode);
            SceneView.lastActiveSceneView.FrameSelected();
        }

        /// <summary>
        /// Select the previous node.
        /// </summary>
        public void SelectPreviousNode()
        {
            var route = transform.GetComponentInParent<Route>();
            var id = route.Nodes.IndexOf(this);

            GameObject nextNode = null;
            if (id == 0)
            {
                nextNode = route.Nodes.Last().gameObject;
            }
            else
            {
                nextNode = route.Nodes[id - 1].gameObject;
            }
            UnitySceneUtils.Select(nextNode);
            SceneView.lastActiveSceneView.FrameSelected();
        }
    }
}