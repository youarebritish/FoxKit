namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using OdinSerializer;
    using OdinSerializer.Utilities;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// Encapsulates current state of the Data List window, including open DataSets and scene proxies.
    /// </summary>
    [CreateAssetMenu(menuName = "FoxKit/Config/Data List Window State", order = 6)]
    public class DataListWindowState : SerializedScriptableObject
    {

        /// <summary>
        /// GUID of the active DataSet.
        /// </summary>
        [HideInInspector, SerializeField]
        private string activeDataSetGuid;

        /// <summary>
        /// The outer dictionary is keyed by DataSet GUID, and returns a dictionary of that DataSet's scene proxies, keyed by entity name.
        /// </summary>
        [OdinSerialize]
        private readonly DoubleLookupDictionary<string, string, SceneProxy> sceneProxies = new DoubleLookupDictionary<string, string, SceneProxy>();

        /// <summary>
        /// GUID of the active DataSet.
        /// </summary>
        public string ActiveDataSetGuid
        {
            get
            {
                return this.activeDataSetGuid;
            }

            set
            {
                this.activeDataSetGuid = value;
            }
        }

        /// <summary>
        /// What to do with a SceneProxy GameObject when deleting the SceneProxy record.
        /// </summary>
        public enum DestroyGameObject
        {
            /// <summary>
            /// Destroy the GameObject.
            /// </summary>
            Destroy,

            /// <summary>
            /// Don't destroy the GameObject.
            /// </summary>
            DontDestroy
        }

        /// <summary>
        /// Clears the state.
        /// </summary>
        public void ClearState()
        {
            this.sceneProxies.Clear();
            this.ActiveDataSetGuid = null;
        }

        /// <summary>
        /// Create a SceneProxy for an Entity.
        /// </summary>
        /// <param name="dataSetGuid">GUID of the DataSetAsset containing the Entity.</param>
        /// <param name="entityName">Name of the Entity.</param>
        /// <returns>The new SceneProxy.</returns>
        public SceneProxy CreateSceneProxyForEntity(string dataSetGuid, string entityName)
        {
            var gameObject = new GameObject(entityName);
            var sceneProxy = gameObject.AddComponent<SceneProxy>();

            var asset = AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(dataSetGuid));
            var entity = asset.GetDataSet().GetData(entityName);
            var transformData = entity as TransformData;

            Assert.IsNotNull(transformData);

            sceneProxy.Entity = transformData;
            sceneProxy.Asset = asset;
            
            Dictionary<string, SceneProxy> dataSetSceneProxies;
            if (this.sceneProxies.TryGetValue(dataSetGuid, out dataSetSceneProxies))
            {
                Assert.IsFalse(dataSetSceneProxies.ContainsKey(entityName));
                dataSetSceneProxies.Add(entityName, sceneProxy);
            }
            else
            {
                dataSetSceneProxies = new Dictionary<string, SceneProxy> { { entityName, sceneProxy } };
                this.sceneProxies.Add(dataSetGuid, dataSetSceneProxies);
            }

            return sceneProxy;
        }

        /// <summary>
        /// Deletes an Entity's SceneProxy.
        /// </summary>
        /// <param name="dataSetGuid">GUID of the DataSetAsset containing the Entity.</param>
        /// <param name="entityName">Name of the Entity.</param>
        /// <param name="destroyGameObject">Whether the SceneProxy GameObject should be destroyed or not.</param>
        public void DeleteSceneProxy(string dataSetGuid, string entityName, DestroyGameObject destroyGameObject)
        {
            Dictionary<string, SceneProxy> dataSetSceneProxies;
            if (!this.sceneProxies.TryGetValue(dataSetGuid, out dataSetSceneProxies))
            {
                return;
            }

            SceneProxy sceneProxy;
            if (!dataSetSceneProxies.TryGetValue(entityName, out sceneProxy))
            {
                return;
            }

            dataSetSceneProxies.Remove(entityName);

            if (dataSetSceneProxies.Count == 0)
            {
                this.sceneProxies.Remove(dataSetGuid);
            }

            if (destroyGameObject == DestroyGameObject.Destroy && sceneProxy != null)
            {
                GameObject.DestroyImmediate(sceneProxy.gameObject);
            }
        }

        /// <summary>
        /// Finds the scene proxy for a given Entity, or null if not found.
        /// </summary>
        /// <param name="dataSetGuid">GUID of the Entity's containing DataSetAsset.</param>
        /// <param name="entityName">Name of the Entity whose scene proxy to find.</param>
        /// <returns>The scene proxy, or null if none was found.</returns>
        public SceneProxy FindSceneProxyForEntity(string dataSetGuid, string entityName)
        {
            Dictionary<string, SceneProxy> dataSetSceneProxies;
            if (!this.sceneProxies.TryGetValue(dataSetGuid, out dataSetSceneProxies))
            {
                Debug.LogError($"Unable to find scene proxy for entity {entityName} in DataSet {AssetDatabase.GUIDToAssetPath(dataSetGuid)}");
                return null;
            }

            SceneProxy proxy;
            if (dataSetSceneProxies.TryGetValue(entityName, out proxy))
            {
                return proxy;
            }

            Debug.LogError($"Unable to find scene proxy for entity {entityName} in DataSet {AssetDatabase.GUIDToAssetPath(dataSetGuid)}");
            return null;
        }
    }
}