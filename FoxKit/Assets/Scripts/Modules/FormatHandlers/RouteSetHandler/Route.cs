using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class Route : MonoBehaviour
{
    public List<RouteNode> Nodes = new List<RouteNode>();

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        RouteNode previousNode = null;
        foreach (var node in Nodes)
        {
            if (previousNode == null)
            {
                previousNode = node;
                continue;
            }
            Gizmos.DrawLine(previousNode.transform.position, node.transform.position);
            previousNode = node;
        }
    }
}
