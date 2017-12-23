using UnityEditor;

namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using UnityEngine;

    public class RouteEditorWindow : EditorWindow
    {
        [MenuItem("FoxKit/Tools/Route Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(RouteEditorWindow), false, "Route Editor");
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            //GUILayout.
            if (GUILayout.Button("New RouteSet"))
            {
            }
            if (GUILayout.Button("New Route"))
            {
            }
            if (GUILayout.Button("Open Route"))
            {
            }
            if (GUILayout.Button("Save Route"))
            {
            }
            if (GUILayout.Button("Export RouteSet"))
            {
            }
            GUILayout.EndHorizontal();
        }
    }
}