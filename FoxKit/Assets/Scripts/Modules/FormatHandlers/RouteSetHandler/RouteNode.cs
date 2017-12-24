using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class RouteNode : MonoBehaviour
{
    public RouteEvent EdgeEvent;
    public List<RouteEvent> Events;
}