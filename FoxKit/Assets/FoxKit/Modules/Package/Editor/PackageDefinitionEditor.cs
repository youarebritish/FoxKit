namespace FoxKit.Modules.Archive.Importer
{
    using System.Collections.Generic;

    using FoxKit.Utils;

    using Rotorz.Games.Collections;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    [CustomEditor(typeof(PackageDefinition))]
    public class PackageDefinitionEditor : Editor
    {
        private ReorderableListControl listControl;
        private IReorderableListAdaptor listAdapter;

        private List<Object> entries;

        private void OnEnable()
        {
            var package = this.target as PackageDefinition;
            this.entries = package.Entries;

            // Create list control and optionally pass flags into constructor.
            this.listControl = new ReorderableListControl();
            this.listAdapter = new GenericListAdaptor<UnityEngine.Object>(this.entries, this.DrawItem, 16.0f);
        }

        private Object DrawItem(Rect position, Object item)
        {
            return EditorGUI.ObjectField(position, item, typeof(UnityEngine.Object), false);
        }

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

            EditorGUI.BeginChangeCheck();
            this.listControl.Draw(this.listAdapter);

            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }

            if (ReorderableListGUI.IndexOfChangedItem == -1)
            {
                return;
            }

            Debug.Log(ReorderableListGUI.IndexOfChangedItem);
            var dataSet = package.Entries[ReorderableListGUI.IndexOfChangedItem] as EntityFileAsset;
            if (dataSet != null)
            {
                dataSet.PackageGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this.target));
            }
        }
    }
}