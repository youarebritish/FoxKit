using System.Collections.Generic;

using UnityEngine;

public class RouteEvent : MonoBehaviour
{
    public string Type;

    [Tooltip("When exporting, treat the event's name as a hash instead of a string literal. Use if its true name is unknown.")]
    public bool TreatTypeAsHash;

    [Tooltip("Optional, to aid readability in Unity. Has no effect ingame.")]
    public string Name;

    [Tooltip("There must be exactly 10 parameters.")]
    public List<uint> Params = new List<uint>(10);

    [Tooltip("Must be a maximum of four characters.")]
    public string Snippet;
}
