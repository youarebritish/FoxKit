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
            var entriesProperty = this.serializedObject.FindProperty("Entries");

            // Create list control and optionally pass flags into constructor.
            this.listControl = new ReorderableListControl();

            // Subscribe to events for item insertion and removal.
            this.listControl.ItemInserted += this.OnItemInserted;
            this.listControl.ItemRemoving += this.OnItemRemoving;

            this.listAdapter = new SerializedPropertyAdaptor(entriesProperty);
        }

        private void OnDisable()
        {
            if (this.listControl == null)
            {
                return;
            }

            this.listControl.ItemInserted -= this.OnItemInserted;
            this.listControl.ItemRemoving -= this.OnItemRemoving;
        }

        private void OnItemInserted(object sender, ItemInsertedEventArgs args)
        {
            var item = this.entries[args.ItemIndex];
            if (args.WasDuplicated)
            {
                return;
            }

            if (!(item is DataSetAsset))
            {
                return;
            }

            var dataSet = item as DataSetAsset;
            dataSet.PackageGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this.target));
        }

        private void OnItemRemoving(object sender, ItemRemovingEventArgs args)
        {
            var item = this.entries[args.ItemIndex];

            if (!(item is DataSetAsset))
            {
                return;
            }

            var dataSet = item as DataSetAsset;
            dataSet.PackageGuid = null;
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
            this.listControl.Draw(this.listAdapter);
        }
    }
}