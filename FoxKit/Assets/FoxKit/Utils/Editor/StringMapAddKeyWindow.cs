using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class StringMapAddKeyWindow : EditorWindow
{
    private string newKey = string.Empty;
    private List<string> invalidKeys;
    private Action<string> onConfirm;

    public static void Create(List<string> invalidKeys, Action<string> onConfirm)
    {
        var window = ScriptableObject.CreateInstance(typeof(StringMapAddKeyWindow)) as StringMapAddKeyWindow;
        window.titleContent = new GUIContent("Add StringMap Key");
        window.minSize = new Vector2(250, 90);
        window.maxSize = new Vector2(400, 90);
        window.invalidKeys = invalidKeys;
        window.onConfirm = onConfirm;
        window.ShowAuxWindow();
    }

    void OnGUI()
    {
        this.newKey = EditorGUILayout.TextField(this.newKey);

        if (string.IsNullOrEmpty(this.newKey))
        {
            GUI.enabled = false;
        }
        else if (this.invalidKeys.Contains(this.newKey))
        {
            EditorGUILayout.HelpBox("The given key is already present in the StringMap.", MessageType.Error);
            GUI.enabled = false;
        }
        else
        {
            GUI.enabled = true;
        }

        if (!GUILayout.Button("Insert", GUILayout.ExpandWidth(false)))
        {
            return;
        }

        this.onConfirm(this.newKey);
        this.Close();
    }
}
