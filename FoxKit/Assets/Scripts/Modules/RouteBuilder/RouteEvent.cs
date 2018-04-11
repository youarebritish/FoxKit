using System.Collections.Generic;

using UnityEngine;

public class RouteEvent : MonoBehaviour
{
    public RouteEventType Type = RouteEventType.None;

    [Tooltip("When exporting, treat the event's name as a hash instead of a string literal. Use if its true name is unknown.")]
    public bool TreatTypeAsHash;

    [HideInInspector]
    public string Name;

    [Tooltip("There must be exactly 10 parameters.")]
    public List<uint> Params = new List<uint> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    [Tooltip("Must be a maximum of four characters.")]
    public string Snippet = string.Empty;

    public static RouteEventType ParseEventType(string eventTypeString)
    {
        uint hash;
        if (uint.TryParse(eventTypeString, out hash))
        {
            return (RouteEventType)System.Enum.Parse(typeof(RouteEventType), eventTypeString.Insert(0, "e"));
        }
        else
        {
            return (RouteEventType)System.Enum.Parse(typeof(RouteEventType), eventTypeString);
        }
    }

    public static string EventTypeToString(RouteEventType eventType)
    {
        var typeString = eventType.ToString();
        if (typeString.StartsWith("e"))
        {
            return typeString.Remove(0, 1);
        }
        return typeString;
    }
}
