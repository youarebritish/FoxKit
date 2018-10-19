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
            var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(assetPath)?.GetDataSet();

            // TODO: Only do this if loaded
            dataSet?.UnloadAllEntities();
            return AssetDeleteResult.DidNotDelete;
        }
    }
}