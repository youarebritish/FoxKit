namespace FoxKit.Modules.RouteBuilder.Editor
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom editor for RouteNodes.
    /// </summary>
    [CustomEditor(typeof(RouteNode))]
    public class RouteNodeEditor : Editor
    {        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            var node = this.target as RouteNode;

            Rotorz.Games.Collections.ReorderableListGUI.Title("Node Events");
            Rotorz.Games.Collections.ReorderableListGUI.ListField(node.Events, this.CustomListItem, this.DrawEmpty);

            EditorUtility.SetDirty(target);
        }

        private RouteNodeEvent CustomListItem(Rect position, RouteNodeEvent itemValue)
        {
            return EditorGUI.ObjectField(position, itemValue, typeof(RouteNodeEvent)) as RouteNodeEvent;
        }

        private void DrawEmpty()
        {
            GUILayout.Label("Node has no node events.", EditorStyles.miniLabel);
        }
    }
}