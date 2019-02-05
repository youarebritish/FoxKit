namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;

    using FmdlStudio.Scripts.MonoBehaviours;

    using FoxKit.Modules.DataSet.Fox.FoxCore;

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
        /// Models to ignore and consider not part of any DataSet. Any models already in the scene when the first DataSet is opened will be ignored.
        /// </summary>
        [OdinSerialize]
        private readonly HashSet<FoxModel> ignoredModels = new HashSet<FoxModel>();

        /// <summary>
        /// The Entity currently open in the Entity tab.
        /// </summary>
        [OdinSerialize]
        private Entity inspectedEntity;

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
        /// Gets or sets the Entity selected in the Entity window.
        /// </summary>
        public Entity InspectedEntity
        {
            get
            {
                return this.inspectedEntity;
            }
            set
            {
                this.inspectedEntity = value;
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
            this.ClearIgnoredModels();
        }

        /// <summary>
        /// Ignores a model. An ignored model will never have a SceneProxy created for it.
        /// </summary>
        /// <param name="model">The model to ignore.</param>
        public void IgnoreModel(FoxModel model)
        {
            this.ignoredModels.Add(model);
        }

        /// <summary>
        /// Empties the set of ignored models.
        /// </summary>
        public void ClearIgnoredModels()
        {
            this.ignoredModels.Clear();
        }

        /// <summary>
        /// Returns true if the given model is ignored.
        /// </summary>
        /// <param name="model">The model to check.</param>
        /// <returns>True if the given model is ignored.</returns>
        public bool IsModelIgnored(FoxModel model)
        {
            return this.ignoredModels.Contains(model);
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
            gameObject.transform.position = SceneView.lastActiveSceneView.pivot;

            var sceneProxy = gameObject.AddComponent<SceneProxy>();

            var asset = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(AssetDatabase.GUIDToAssetPath(dataSetGuid));
            var entity = asset.GetDataSet().GetData(entityName);

            Assert.IsNotNull(entity);

            sceneProxy.Entity = entity;
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
                return null;
            }

            SceneProxy proxy;
            if (dataSetSceneProxies.TryGetValue(entityName, out proxy))
            {
                return proxy;
            }

            return null;
        }
    }
}