namespace FoxKit.Modules.DataSet.Fox.FoxGameKit
{
    using System.Linq;

    using FoxKit.Modules.Terrain;

    using UnityEditor;

    using UnityEngine;

    public partial class TerrainBlock
    {
        /// <inheritdoc />
        public override void OnLoaded(CreateSceneProxyDelegate createSceneProxy)
        {
            base.OnLoaded(createSceneProxy);
            this.CreateSceneProxy(createSceneProxy);
        }

        /// <inheritdoc />
        public override void OnUnloaded(DestroySceneProxyDelegate destroySceneProxy)
        {
            base.OnUnloaded(destroySceneProxy);
            this.DestroySceneProxy(destroySceneProxy);
        }

        protected virtual void CreateSceneProxy(CreateSceneProxyDelegate createSceneProxy)
        {
            var sceneProxy = createSceneProxy();
            sceneProxy.transform.position = this.pos;
            sceneProxy.transform.Rotate(new Vector3(0, -90));

            var terrainTileSceneProxy = sceneProxy.gameObject.AddComponent<TerrainTileSceneProxy>();

            // TODO HACK until parenting issue with htre importing is fixed
            var assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this.filePtr));
            var htreAsset = (TerrainTileAsset)assets.First(asset => asset is TerrainTileAsset);

            var prefs = TerrainPreferences.Instance;
            terrainTileSceneProxy.Initialize(prefs.TerrainTileMesh, prefs.TerrainTileMaterial, htreAsset.Heightmap, htreAsset.MaterialWeightMap);
        }

        protected virtual void DestroySceneProxy(DestroySceneProxyDelegate destroySceneProxy)
        {
            destroySceneProxy();
        }
    }
}
