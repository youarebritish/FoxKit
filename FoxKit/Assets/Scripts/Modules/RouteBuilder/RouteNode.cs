namespace FoxKit.Modules.RouteBuilder
{
    using FoxKit.Utils;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// A point in a Route for an AI agent to navigate to.
    /// </summary>
    [System.Serializable]
    public class RouteNode : MonoBehaviour
    {
        /// <summary>
        /// Event for AI agents to perform on the way to this node.
        /// </summary>
        [Tooltip("Event for AI agents to perform on the way to this node.")]
        public RouteEvent EdgeEvent;

        /// <summary>
        /// Events for AI agents to perform at this node.
        /// </summary>
        [Tooltip("Events for AI agents to perform at this node.")]
        public List<RouteEvent> Events;

        /// <summary>
        /// Context menu to add a new node to the Route.
        /// </summary>
        [ContextMenu("Add Node", false, 0)]
        private void AddNewNode()
        {
            CreateRouteSetEditor.CreateNewNode(transform.GetComponentInParent<Route>());
        }

        /// <summary>
        /// Context menu to add a new event to the node.
        /// </summary>
        [ContextMenu("Add Event", false, 1)]
        private void AddNewEvent()
        {
            var @event = CreateRouteSetEditor.CreateNewNodeEvent(gameObject);
            this.Events.Add(@event);
        }

        /// <summary>
        /// Select the next node.
        /// </summary>
        [ContextMenu("Next Node", false, 100)]
        private void SelectNextNode()
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
        }

        /// <summary>
        /// Select the previous node.
        /// </summary>
        [ContextMenu("Previous Node", false, 101)]
        private void SelectPreviousNode()
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
        }
    }
}