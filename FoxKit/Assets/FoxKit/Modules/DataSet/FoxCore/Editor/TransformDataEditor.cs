using UnityEditor;

namespace FoxKit.Modules.DataSet.FoxCore.Editor
{
    using UnityEngine;

    [CustomEditor(typeof(TransformData), true)]
    public class TransformDataEditor : DataEditor
    {
        public override void OnInspectorGUI()
        {
            var transform = ((TransformData)this.target).SceneProxyTransform;
            var transformEntity = new SerializedObject(transform);
            
            transformEntity.Update();

            EditorGUILayout.PropertyField(transformEntity.FindProperty("m_LocalPosition"), new GUIContent("Position"), true);
            transform.localRotation = Quaternion.Euler(EditorGUILayout.Vector3Field(
                new GUIContent("Rotation"),
                transformEntity.FindProperty("m_LocalRotation").quaternionValue.eulerAngles));
            EditorGUILayout.PropertyField(transformEntity.FindProperty("m_LocalScale"), new GUIContent("Scale"), true);

            transformEntity.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}