using System.Collections.Generic;
using System.Linq;

using Rotorz.Games.Collections;

using UnityEditor;

using UnityEngine;

public static class StringMapGUI
{
    public delegate T ItemDrawerWithKey<T>(Rect position, T item, string key);

    public static void StringMapField<TValue>(IDictionary<string, TValue> dictionary, ReorderableListControl.ItemDrawer<TValue> drawItem)
    {
        // Draw entries.
        string keyToRemove = null;

        var changedEntries = new List<KeyValuePair<string, TValue>>();
        foreach (var kvp in dictionary)
        {
            EditorGUILayout.BeginHorizontal();

            var wasGUIenabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.TextField(kvp.Key);
            GUI.enabled = wasGUIenabled;

            var position = EditorGUILayout.GetControlRect(false, 16);

            var currentValue = kvp.Value;
            var newValue = drawItem(position, currentValue);

            // Draw remove button.
            if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
            {
                keyToRemove = kvp.Key;
            }
            else
            {
                changedEntries.Add(new KeyValuePair<string, TValue>(kvp.Key, newValue));
            }

            EditorGUILayout.EndHorizontal();
        }

        // Remove whichever key the user selected, if any.
        if (keyToRemove != null)
        {
            dictionary.Remove(keyToRemove);
        }

        // Update any changed entries.
        foreach (var kvp in changedEntries)
        {
            dictionary[kvp.Key] = kvp.Value;
        }

        // Draw add entry section.
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
        {
            StringMapAddKeyWindow.Create(
                dictionary.Keys.ToList(),
                newKey => dictionary.Add(newKey, default(TValue)));
        }

        EditorGUILayout.EndHorizontal();
    }

    // TODO: Fix repeated code
    public static void StringMapFieldWithKey<TValue>(IDictionary<string, TValue> dictionary, ItemDrawerWithKey<TValue> drawItem)
    {
        // Draw entries.
        string keyToRemove = null;

        var changedEntries = new List<KeyValuePair<string, TValue>>();
        foreach (var kvp in dictionary)
        {
            EditorGUILayout.BeginHorizontal();

            var wasGUIenabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.TextField(kvp.Key);
            GUI.enabled = wasGUIenabled;

            var position = EditorGUILayout.GetControlRect(false, 16);

            var currentValue = kvp.Value;
            var newValue = drawItem(position, currentValue, kvp.Key);

            // Draw remove button.
            if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
            {
                keyToRemove = kvp.Key;
            }
            else
            {
                changedEntries.Add(new KeyValuePair<string, TValue>(kvp.Key, newValue));
            }

            EditorGUILayout.EndHorizontal();
        }

        // Remove whichever key the user selected, if any.
        if (keyToRemove != null)
        {
            dictionary.Remove(keyToRemove);
        }

        // Update any changed entries.
        foreach (var kvp in changedEntries)
        {
            dictionary[kvp.Key] = kvp.Value;
        }

        // Draw add entry section.
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
        {
            StringMapAddKeyWindow.Create(
                dictionary.Keys.ToList(),
                newKey => dictionary.Add(newKey, default(TValue)));
        }

        EditorGUILayout.EndHorizontal();
    }
}