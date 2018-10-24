namespace FoxKit.Modules.RailBuilder.Editor
{
    using UnityEngine;
    using UnityEditor;
    using FoxKit.Modules.RailBuilder.Exporter;
    using System.IO;

    using Rotorz.Games.Collections;

    /// <summary>
    /// Custom editor for MaterialDatabases.
    /// </summary>
    [CustomEditor(typeof(RailUniqueIdSet))]
    public class RailUniqueIdSetEditor : Editor
    {
        private SerializedProperty idsProperty;

        private void OnEnable()
        {
            this.idsProperty = this.serializedObject.FindProperty("Ids");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Export frld"))
            {
                var myTarget = (RailUniqueIdSet)this.target;

                var exportPath = EditorUtility.SaveFilePanel(
                    "Export frld",
                    string.Empty,
                    this.target.name + ".frld",
                    "frld");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }

                RailUniqueIdSetExporter.ExportRailUniqueIdSet(myTarget.Ids, exportPath);
            }
            else if (GUILayout.Button("Export txt"))
            {
                var myTarget = (RailUniqueIdSet)this.target;

                var exportPath = EditorUtility.SaveFilePanel(
                    "Export txt",
                    string.Empty,
                    this.target.name + ".txt",
                    "txt");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }

                using (var writer = new StreamWriter(exportPath))
                {
                    foreach (var id in myTarget.Ids)
                    {
                        writer.WriteLine(id);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            ReorderableListGUI.ListField(this.idsProperty, ReorderableListFlags.ShowIndices);
        }
    }
}