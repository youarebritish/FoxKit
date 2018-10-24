namespace FoxKit.Core
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using OneLine;
    using RectEx;

    [CustomPropertyDrawer(typeof(PathFileNameCode64StringPair))]
    public class PathFileNameCode64StringPairPropertyDrawer : OneLinePropertyDrawer
    {
        private ComplexFieldDrawer directoryDrawer;
        private RootDirectoryDrawer rootDirectoryDrawer;

        private SlicesCache cache;
        private InspectorUtil inspectorUtil;
        private ArraysSizeObserver arraysSizeObserver;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            directoryDrawer = new DirectoryDrawer(GetDrawer);
            rootDirectoryDrawer = new RootDirectoryDrawer(GetDrawer);

            inspectorUtil = new InspectorUtil();
            ResetCache();
            Undo.undoRedoPerformed += ResetCache;
            arraysSizeObserver = new ArraysSizeObserver();

            if (Event.current.type == EventType.Layout) { return; } // In [Expandable] popup it happens
            if (inspectorUtil.IsOutOfScreen(position)) { return; } // Culling

            if (arraysSizeObserver.IsArraySizeChanged(property)) { ResetCache(); }

            rootDirectoryDrawer.RootDepth = property.depth;
            directoryDrawer.RootDepth = property.depth;
            position = rootDirectoryDrawer.DrawPrefixLabel(position, property);

            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position = DrawHeaderIfNeed(position, property);
            DrawLine(position, property, (slice, rect) => slice.Draw(rect));

            EditorGUI.indentLevel = indentLevel;
        }

        private void DrawLine(Rect position, SerializedProperty property, Action<Slice, Rect> draw)
        {
            var slices = cache[property];
            var rects = position.Row(slices.Weights, slices.Widthes, 2);

            rects[1].width = rects[2].width;

            if (property.FindPropertyRelative("_isUnhashed").enumValueIndex == 0)
            {
                draw(slices[0], rects[0]);
            }
            else
            {
                draw(slices[1], rects[0]);
            }

            draw(slices[2], rects[1]);
        }

        private void ResetCache()
        {
            if (cache == null || cache.IsDirty)
            {
                cache = new SlicesCache(rootDirectoryDrawer.AddSlices);
            }
        }
    }
}