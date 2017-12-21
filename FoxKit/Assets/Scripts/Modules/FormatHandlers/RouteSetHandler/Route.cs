using System.Collections.Generic;

using FoxKit.Modules.FormatHandlers.RouteSetHandler;

using UnityEngine;

[System.Serializable]
public class Route : MonoBehaviour
{
    public List<RouteNode> Nodes = new List<RouteNode>();

    void OnDrawGizmos()
    {
        Gizmos.color = RouteSetImporterPreferences.Instance.EdgeColor;
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

        // Connect first and last nodes.
        if (this.Nodes.Count > 2)
        {
            Gizmos.DrawLine(this.Nodes[this.Nodes.Count - 1].transform.position, this.Nodes[0].transform.position);
        }
    }
}
