namespace FoxKit.Modules.PartsBuilder.FormVariation.Editor
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using FoxKit.Core;

    public class StringSelectPopup : EditorWindow
    {
        private string searchString;
        public string Result { get; private set; }

        private string[] result;

        private Vector2 scrollPosition;

        //[MenuItem("Window/Example Popup %e")]
        public void InitPopup(Rect rect, string[] search, string[] output)
        {
            result = output;
            var screenPoints = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
            var rectCopy = rect;
            rectCopy.x = screenPoints.x;
            rectCopy.y = screenPoints.y;
            this.ShowAsDropDown(rectCopy, new Vector2(rect.width, 250));
            //this.position = new Rect(rect.x, rect.y, 0f, 0f); //rect;
        }

        private void OnGUI()
        {
            var toolbarStyle = GUI.skin.FindStyle("Toolbar");
            var searchStyle = GUI.skin.FindStyle("ToolbarSeachTextField");
            var cancelStyle = GUI.skin.FindStyle("ToolbarSeachCancelButton");

            GUILayout.BeginHorizontal(toolbarStyle);
            searchString = EditorGUILayout.TextField(searchString, searchStyle);
            if (GUILayout.Button("", cancelStyle))
            {
                // Remove focus if cleared
                searchString = "";
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginScrollView(scrollPosition);
            //var controlRect = EditorGUILayout.GetControlRect();
            //controlRect.x = 0;
            //controlRect.y = toolbarStyle.fixedHeight;
            //controlRect.height = this.maxSize.y - controlRect.yMin;
            //controlRect.width = this.maxSize.x;
            //if (GUI.Button(controlRect, "Test"))
            //{
            //    result = "Hello";
            //    this.Close();
            //}
            if (GUILayout.Button("Test"))
            {
                result[0] = "Hello";
                this.Close();
            }
            GUILayout.EndScrollView();
        }
    }


    /// <summary>
    /// #NEW#
    /// </summary
    public static class FormVariationEditorUtils
    {
        public static string[] ObjectStringField<T>(FoxStringRef current, Rect rect, Func<T, string[]> getListFunc) where T : UnityEngine.Object
        {
            return ObjectStringField(current, rect, FireAndForgetObjectField<T>(rect), getListFunc);
        }

        public static T FireAndForgetObjectField<T>(Rect rect) where T : UnityEngine.Object
        {
            return (T)EditorGUI.ObjectField(rect, null, typeof(T), allowSceneObjects: false);
        }

        public static string[] ObjectStringField<T>(string[] current, Rect rect, T source, Func<T, string[]> getListFunc) where T : UnityEngine.Object
        {
            if (source != null)
            {
                var window = ScriptableObject.CreateInstance<StringSelectPopup>();
                window.InitPopup(rect, getListFunc(source), current);
            }

            return current;
        }
    }
}