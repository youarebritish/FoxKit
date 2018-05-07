using UnityEditor;

namespace FoxKit.Modules.DataSet.FoxCore.Editor
{
    using UnityEngine;

    [CustomEditor(typeof(TransformData), true)]
    public class TransformDataEditor : DataEditor
    {
        private void OnEnable()
        {
            //ActiveEditorTracker.sharedTracker.isLocked = true;
        }

        private void OnDisable()
        {
            //ActiveEditorTracker.sharedTracker.isLocked = false;
        }
    }
}