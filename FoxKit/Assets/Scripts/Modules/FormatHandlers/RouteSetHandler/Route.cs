using System.Collections.Generic;
using System.Linq;

using FoxKit.Modules.FormatHandlers.RouteSetHandler;

using UnityEditor;

using UnityEngine;

[System.Serializable]
public class Route : MonoBehaviour
{
    public List<RouteNode> Nodes = new List<RouteNode>();

    void OnDrawGizmos()
    {
        Gizmos.color = RouteSetImporterPreferences.Instance.EdgeColor;
        RouteNode previousNode = null;
        foreach (var node in this.Nodes)
        {
            Gizmos.color = RouteSetImporterPreferences.Instance.NodeColor;
            if (!Selection.Contains(gameObject))
            {
                Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, Gizmos.color.a * 0.25f);
            }

            Gizmos.DrawWireSphere(this.transform.position, RouteSetImporterPreferences.Instance.NodeSize);

            if (previousNode == null)
            {
                previousNode = node;
                continue;
            }

            Gizmos.color = RouteSetImporterPreferences.Instance.EdgeColor;
            if (!Selection.Contains(gameObject))
            {
                Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, Gizmos.color.a * 0.25f);
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

    private bool IsRouteSelected()
    {
        if (Selection.Contains(gameObject))
        {
            return true;
        }
        if (Selection.Contains(this.transform.parent.gameObject))
        {
            return true;
        }
        return this.Nodes.Any(node => Selection.Contains(node.gameObject));
    }
}
