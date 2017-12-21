using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RouteEvent
{
    public string Name;
    public List<uint> Params = new List<uint>(10);
    public string Snippet;
}
