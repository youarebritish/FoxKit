using System.Collections.Generic;

using FoxKit.Modules.FormatHandlers.RouteSetHandler;

using UnityEngine;

[System.Serializable]
public class RouteNode : MonoBehaviour
{
    public RouteEvent EdgeEvent;
    public List<RouteEvent> Events;

    void OnDrawGizmos()
    {
        Gizmos.color = RouteSetImporterPreferences.Instance.NodeColor;
        Gizmos.DrawWireSphere(this.transform.position, RouteSetImporterPreferences.Instance.NodeSize);
    }
}