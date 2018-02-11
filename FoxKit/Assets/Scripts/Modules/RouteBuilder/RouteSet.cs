using System.Collections.Generic;
using UnityEngine;
using static FoxKit.Modules.RouteBuilder.Importer.EventFactory;

public class RouteSet : MonoBehaviour
{
    public List<Route> Routes;
    public Dictionary<FoxLib.Tpp.RouteSet.RouteEvent, RouteEvent> EdgeEvents = new Dictionary<FoxLib.Tpp.RouteSet.RouteEvent, RouteEvent>();
    public GameObject EdgeEventsContainer;

    public RouteEvent RegisterRouteEvent(FoxLib.Tpp.RouteSet.RouteEvent data, CreateEventDelegate createEvent)
    {
        RouteEvent edgeEvent = null;
        if (EdgeEvents.TryGetValue(data, out edgeEvent))
        {
            return edgeEvent;
        }
        else
        {
            var edgeEventGameObject = new GameObject();
            edgeEventGameObject.transform.SetParent(EdgeEventsContainer.transform);

            edgeEvent = createEvent(edgeEventGameObject, data);
            edgeEventGameObject.name = edgeEvent.Name;

            EdgeEvents.Add(data, edgeEvent);
            return edgeEvent;
        }
    }
}
