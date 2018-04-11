using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Collection of utility functions for working with a Unity scene.
/// </summary>
public static class UnitySceneUtils
{
    /// <summary>
    /// Change the current selection.
    /// </summary>
    /// <param name="go">The GameObject to select.</param>
    public static void Select(GameObject go)
    {
        var newSelection = new GameObject[] { go };
        Selection.objects = newSelection;
    }
}
