namespace FoxKit.Modules.Atmosphere.DataSet.Editor
{
    using System.IO;

    using UnityEngine;
    using UnityEditor;

    using FoxKit.Utils;
    using FoxKit.Modules.DataSet.Exporter;

    /// <summary>
    /// Custom editor for EntityFileAsset.
    /// </summary>
    [CustomEditor(typeof(EntityFileAsset), true)]
    public class DataSetAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var asset = (EntityFileAsset)this.target;
            if (asset.IsReadOnly)
            {
                FoxKitUiUtils.ReadOnlyWarningAndButton(asset, thisAsset => thisAsset.IsReadOnly = false);
            }
            if (string.IsNullOrEmpty(asset.PackageGuid))
            {
                EditorGUILayout.HelpBox("This DataSet does not belong to a package. Import or create a Package Definition and add this DataSet to it.", MessageType.Warning);
            }

            this.DrawDefaultInspector();
        }
    }
}