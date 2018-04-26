//namespace FoxKit.Modules.FormVariation.Editor
//{
//    using UnityEngine;
//    using UnityEditor;

//    using Rotorz.Games.Collections;

//    [CustomEditor(typeof(FormVariation))]
//    public class FormVariation : Editor
//    {
//		public override void OnInspectorGUI()
//		{
//            ReorderableListGUI.Title("FormVariation");
//		}

//        private void Draw()
//        {
            
//        }

//        private static ShownMeshGroup CustomListItem(Rect position, ShownMeshGroup itemValue)
//        {
//            return EditorGUI.ObjectField(position, itemValue, typeof(ShownMeshGroup)) as ShownMeshGroup;
//        }

//        private static void DrawEmpty(string arrayName)
//        {
//            GUILayout.Label(arrayName + " has no routes.", EditorStyles.miniLabel);
//        }
//	}
//}