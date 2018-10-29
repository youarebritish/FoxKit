namespace FoxKit.Modules.Archive.Importer
{
    using FoxKit.Utils;

    using Rotorz.Games.Collections;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    [CustomEditor(typeof(PackageDefinition))]
    public class PackageDefinitionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var package = this.target as PackageDefinition;
            Assert.IsNotNull(package);

            if (package.IsReadOnly)
            {
                FoxKitUiUtils.ReadOnlyWarningAndButton(package, duplicate => duplicate.IsReadOnly = false);
            }

            package.Type = (PackageDefinition.PackageType)EditorGUILayout.EnumPopup("Type", package.Type);

            ReorderableListGUI.Title("Entries");
            ReorderableListGUI.ListField(package.Entries, CustomListItem);
        }

        private static UnityEngine.Object CustomListItem(Rect position, UnityEngine.Object itemValue)
        {
            return EditorGUI.ObjectField(position, itemValue, typeof(UnityEngine.Object), false);
        }
    }
}