using System.Collections.Generic;

using UnityEngine;

public class RouteEvent : MonoBehaviour
{
    public string Type;

    [Tooltip("Optional, to aid readability in Unity. Has no effect ingame.")]
    public string Name;
    public List<uint> Params = new List<uint>(10);
    public string Snippet;
}
