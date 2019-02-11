namespace FoxKit.Modules.RouteBuilder.Editor
{
    using Rotorz.Games.Collections;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom editor for RouteNodes.
    /// </summary>
    [CustomEditor(typeof(RouteNode))]
    public class RouteNodeEditor : Editor
    {
        private ReorderableListControl listControl;
        private IReorderableListAdaptor listAdaptor;

        public void OnEnable()
        {
            var node = this.target as RouteNode;
            node.Rebuild();

            listControl = new ReorderableListControl();
            listControl.ItemRemoving += this.OnItemRemoving;
            listAdaptor = new GenericListAdaptor<RouteNodeEvent>(node.Events, CustomListItem, ReorderableListGUI.DefaultItemHeight);
        }

        private void OnDisable()
        {
            // Unsubscribe from events
            if (listControl != null)
            {
                listControl.ItemRemoving -= this.OnItemRemoving;
            }
        }

        private void OnItemRemoving(object sender, ItemRemovingEventArgs args)
        {
            var node = this.target as RouteNode;
            RouteNodeEvent item = node.Events[args.ItemIndex];
            DestroyImmediate(item.gameObject);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            var node = this.target as RouteNode;

            Rotorz.Games.Collections.ReorderableListGUI.Title("Node Events");
            listControl.Draw(listAdaptor);

            EditorUtility.SetDirty(target);
        }

        private RouteNodeEvent CustomListItem(Rect position, RouteNodeEvent itemValue)
        {
            return EditorGUI.ObjectField(position, itemValue, typeof(RouteNodeEvent), true) as RouteNodeEvent;
        }

        private void DrawEmpty()
        {
            GUILayout.Label("Node has no node events.", EditorStyles.miniLabel);
        }
    }
}