namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using UnityEditor;

    public class FoxKitAssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        /// <summary>
        /// Before a DataSetAsset is deleted, unload its Entities to clean up any scene proxies.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="rao"></param>
        /// <returns></returns>
        public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions rao)
        {
            const AssetDeleteResult Result = AssetDeleteResult.DidNotDelete;

            var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(assetPath);
            if (dataSet == null)
            {
                return Result;
            }

            // TODO: Refactor and fix this monstrosity

            var wasDataListWindowOpen = DataListWindow.IsOpen;
            var window = DataListWindow.GetInstance();
            var guid = AssetDatabase.AssetPathToGUID(assetPath);

            if (!window.IsDataSetOpen(guid))
            {
                return Result;
            }

            window.RemoveDataSet(guid);

            if (!wasDataListWindowOpen)
            {
                window.Close();
            }

            return Result;
        }
    }
}