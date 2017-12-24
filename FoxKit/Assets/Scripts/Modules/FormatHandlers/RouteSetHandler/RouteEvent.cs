using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[System.Serializable]
public class RouteEvent
{
    public string Name;
    public List<uint> Params = new List<uint>(10);
    public string Snippet;

    /*public override bool Equals(System.Object obj)
    {
        var routeEvent = obj as RouteEvent;
        return routeEvent != null && this == routeEvent;
    }

    public static bool operator ==(RouteEvent x, RouteEvent y)
    {
        if (ReferenceEquals(null, x))
        {
            return ReferenceEquals(null, y);
        }
        if (ReferenceEquals(null, y))
        {
            return false;
        }
        if (x.Name != y.Name)
        {
            return false;
        }
        if (x.Params.Where((t, i) => t != y.Params[i]).Any())
        {
            return false;
        }
        return x.Snippet == y.Snippet;
    }

    public static bool operator !=(RouteEvent x, RouteEvent y)
    {
        return !(x == y);
    }*/
}
