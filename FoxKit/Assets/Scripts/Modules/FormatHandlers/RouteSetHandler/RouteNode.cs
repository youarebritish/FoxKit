using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RouteNode : MonoBehaviour
{
    public RouteEvent EdgeEvent;
    public List<RouteEvent> Events;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}