namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    public class AssetPostprocessor : UnityEditor.AssetPostprocessor
    {

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            // Temporary - Too much of a hassle to properly relink reimported DataSets for now.
            DataListWindow.GetInstance().CloseAllDataSets();
        }
    }
}