namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Utils;
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Custom editor for Routes.
    /// </summary>
    [CustomEditor(typeof(Route))]
    public class RouteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var route = this.target as Route;
            
            var iconAddNode = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_new_node") as Texture;
            var iconParent = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_parent") as Texture;

            Rotorz.Games.Collections.ReorderableListGUI.Title("Tools");
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Add node button
            var addNodeContent = new GUIContent(iconAddNode, "Add a new node.");
            if (GUILayout.Button(addNodeContent, GUILayout.MaxWidth(FoxKitUiUtils.BUTTON_DIMENSION), GUILayout.MaxHeight(FoxKitUiUtils.BUTTON_DIMENSION)))
            {
                route.AddNewNode();
            }

            // Select parent button
            var selectParentContent = new GUIContent(iconParent, "Select parent RouteSet.");
            if (GUILayout.Button(selectParentContent, GUILayout.MaxWidth(FoxKitUiUtils.BUTTON_DIMENSION), GUILayout.MaxHeight(FoxKitUiUtils.BUTTON_DIMENSION)))
            {
                UnitySceneUtils.Select(route.transform.parent.gameObject);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            //EditorGUILayout.Space();

            Rotorz.Games.Collections.ReorderableListGUI.Title("Settings");
            route.Closed = EditorGUILayout.Toggle("Closed", route.Closed);
            route.TreatNameAsHash = EditorGUILayout.Toggle("Treat Name as Hash", route.TreatNameAsHash);

            Rotorz.Games.Collections.ReorderableListGUI.Title("Nodes");
            Rotorz.Games.Collections.ReorderableListGUI.ListField(route.Nodes, this.CustomListItem, this.DrawEmpty);

            EditorUtility.SetDirty(target);
        }

        private RouteNode CustomListItem(Rect position, RouteNode itemValue)
        {
            return EditorGUI.ObjectField(position, itemValue, typeof(RouteNode)) as RouteNode;
        }

        private void DrawEmpty()
        {
            GUILayout.Label("Route has no nodes.", EditorStyles.miniLabel);
        }
    }
}