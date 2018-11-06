namespace FoxKit.Modules.DataSet
{
    using System;

    using FmdlStudio.Scripts.MonoBehaviours;

    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.DataSet.Fox.FoxGameKit;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    using Object = UnityEngine.Object;

    /// <inheritdoc />
    /// <summary>
    /// A scene representation of a FoxKit Entity. Created to visualize spatial Entity data.
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class SceneProxy : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private TransformData entity;

        [SerializeField, HideInInspector]
        private DataSetAsset asset;

        [SerializeField, HideInInspector]
        private string entityName;

        [SerializeField, HideInInspector]
        private string entityDataSetGuid;

        public bool DrawLocatorGizmo = true;

        public TransformData Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
                this.entityName = value.Name;
                this.entityDataSetGuid = value.DataSetGuid;
            }
        }

        public DataSetAsset Asset
        {
            set
            {
                this.asset = value;
            }
        }

        /// <summary>
        /// On serialize, Entity references become stale and must be regenerated.
        /// </summary>
        private void OnEnable()
        {
            if (string.IsNullOrEmpty(this.entityName) || string.IsNullOrEmpty(this.entityDataSetGuid))
            {
                return;
            }

            var dataSetPath = AssetDatabase.GUIDToAssetPath(this.entityDataSetGuid);
            Assert.IsFalse(string.IsNullOrEmpty(dataSetPath));

            var dataSet = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(dataSetPath).GetDataSet();
            Assert.IsNotNull(dataSet);

            this.entity = dataSet.GetData(this.entityName) as TransformData;
        }

        void Update()
        {
            if (this.transform.parent == null)
            {
                return;
            }

            var parentSceneProxy = this.transform.parent.GetComponent<SceneProxy>();
            if (parentSceneProxy == null)
            {
                return;
            }

            this.entity.Parent = parentSceneProxy.entity;
        }

        private readonly static Color LocatorColor = new Color(67.0f/255.0f, 1.0f, 163.0f/255.0f);

        void OnDrawGizmos()
        {
            if (!this.DrawLocatorGizmo)
            {
                return;
            }

            Gizmos.color = LocatorColor;
            Gizmos.DrawLine(this.transform.position - this.transform.forward, this.transform.position + this.transform.forward);
            Gizmos.DrawLine(this.transform.position - this.transform.up, this.transform.position + this.transform.up);
            Gizmos.DrawLine(this.transform.position - this.transform.right, this.transform.position + this.transform.right);
        }

        private Vector3 previousEntityTranslation;

        private Quaternion previousEntityRotation;

        private Vector3 previousEntityScale;

        void OnDrawGizmosSelected()
        {
            // TODO hack
            // Sometimes their positions get out of sync somehow
            /*var foxModel = GetComponentInChildren<FoxModel>();
            if (foxModel != null && foxModel.transform.localPosition != Vector3.zero)
            {
                this.transform.position += foxModel.transform.localPosition;
                foxModel.transform.localPosition = Vector3.zero;
            }*/

            if (this.entity.Transform.Translation != this.previousEntityTranslation)
            {
                this.transform.position = this.entity.Transform.Translation;
                this.previousEntityTranslation = this.transform.position;
            }
            else
            {
                this.entity.Transform.Translation = this.transform.position;
                this.previousEntityTranslation = this.transform.position;
            }
            
            if (this.DrawLocatorGizmo)
            {
                Gizmos.DrawLine(this.transform.position - this.transform.forward, this.transform.position + this.transform.forward);
                Gizmos.DrawLine(this.transform.position - this.transform.up, this.transform.position + this.transform.up);
                Gizmos.DrawLine(this.transform.position - this.transform.right, this.transform.position + this.transform.right);
            }

            EditorUtility.SetDirty(this.asset);
        }
    }
}