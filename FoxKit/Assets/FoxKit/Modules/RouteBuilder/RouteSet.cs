using FoxKit.Modules.RouteBuilder;
using FoxKit.Utils;
using System.Collections.Generic;
using UnityEngine;
using static FoxKit.Modules.RouteBuilder.Importer.EventFactory;

/// <summary>
/// A container for AI Routes.
/// </summary>
[System.Serializable]
public class RouteSet : MonoBehaviour
{
    /// <summary>
    /// Default event type to apply to new node events.
    /// </summary>
    [Tooltip("Default event type to apply to new node events.")]
    public RouteNodeEventType DefaultNodeEventType = RouteNodeEventType.RelaxedIdleAct;

    /// <summary>
    /// Default edge event type to apply to new nodes.
    /// </summary>
    [Tooltip("Default edge event type to apply to new nodes.")]
    public RouteEdgeEventType DefaultEdgeEventType = RouteEdgeEventType.RelaxedWalk;

    /// <summary>
    /// All of the Routes contained within this RouteSet.
    /// </summary>
    public List<Route> Routes = new List<Route>();
         
    /// <summary>
    /// Registers a RouteNodeEvent instance.
    /// </summary>
    /// <param name="data">Raw data associated with the event.</param>
    /// <param name="parent">Parent object of the event.</param>
    /// <param name="createEvent">Function to create a new event.</param>
    /// <returns>The new, registered route event instance.</returns>
    public RouteNodeEvent RegisterRouteNodeEvent(FoxLib.Tpp.RouteSet.RouteEvent data, Transform parent, CreateEventDelegate createEvent)
    {
        var eventGameObject = new GameObject();
        eventGameObject.transform.position = parent.transform.position;
        eventGameObject.transform.SetParent(parent);

        var eventInstance = createEvent(eventGameObject, data);
        eventGameObject.name = eventInstance.Name;

        return eventInstance as RouteNodeEvent;
    }

    /// <summary>
    /// Registers a route event instance.
    /// </summary>
    /// <param name="data">Raw data associated with the event.</param>
    /// <param name="parent">Parent object of the event.</param>
    /// <param name="createEvent">Function to create a new event.</param>
    /// <returns>The new, registered route event instance.</returns>
    public RouteEdgeEvent RegisterRouteEdgeEvent(FoxLib.Tpp.RouteSet.RouteEvent data, Transform parent, CreateEventDelegate createEvent)
    {
        var eventGameObject = new GameObject();
        eventGameObject.transform.position = parent.transform.position;
        eventGameObject.transform.SetParent(parent);

        var eventInstance = createEvent(eventGameObject, data);
        eventGameObject.name = eventInstance.Name;

        return eventInstance as RouteEdgeEvent;
    }


    /// <summary>
    /// Registers a route event instance.
    /// </summary>
    /// <param name="data">Raw data associated with the event.</param>
    /// <param name="owner">GameObject for the event to attach to.</param>
    /// <param name="createEvent">Function to create a new event.</param>
    /// <returns>The new, registered route event instance.</returns>
    public RouteEvent RegisterRouteEvent(FoxLib.Tpp.RouteSet.RouteEvent data, GameObject owner, CreateEventDelegate createEvent)
    {
        RouteEvent eventInstance = createEvent(owner, data);
        return eventInstance;
    }

    /// <summary>
    /// Context menu to add a new Route to the RouteSet.
    /// </summary>
    public void AddNewRoute()
    {
        var go = new GameObject();
        go.transform.SetParent(transform);
        go.name = GenerateNewRouteName(gameObject.name, Routes.Count);

        var route = go.AddComponent<Route>();
        Routes.Add(route);

        route.AddNewNode();
    }

    /// <summary>
    /// Generate name for a new Route.
    /// </summary>
    /// <param name="routeSetName">Name of the RouteSet containing the new Route.</param>
    /// <param name="routeCount">Number of Routes already in the RouteSet.</param>
    /// <returns>Name for a new Route.</returns>
    private static string GenerateNewRouteName(string routeSetName, int routeCount)
    {
        return string.Format("rt_{0}_c_{1}", routeSetName, routeCount.ToString("D4"));
    }
}