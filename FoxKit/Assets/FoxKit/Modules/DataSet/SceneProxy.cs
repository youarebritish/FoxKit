namespace FoxKit.Modules.DataSet
{
    using FoxKit.Modules.DataSet.Fox.FoxCore;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// A scene representation of a FoxKit Entity. Created to visualize spatial Entity data.
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode, SelectionBase]
    public class SceneProxy : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Data entity;

        [SerializeField, HideInInspector]
        private EntityFileAsset asset;

        [SerializeField, HideInInspector]
        private string entityName;

        [SerializeField, HideInInspector]
        private string entityDataSetGuid;

        public bool DrawLocatorGizmo = true;

        public Data Entity
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

        public EntityFileAsset Asset
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

            if (this.entity is TransformData)
            {
               ((TransformData)this.entity).Parent = (TransformData)parentSceneProxy.entity;
            }
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
            var transformData = this.entity as TransformData;
            if (transformData == null)
            {
                return;
            }

            if (transformData.Transform.Translation != this.previousEntityTranslation)
            {
                this.transform.position = transformData.Transform.Translation;
                this.previousEntityTranslation = this.transform.position;
            }
            else
            {
                transformData.Transform.Translation = this.transform.position;
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