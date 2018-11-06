namespace FoxKit.Modules.DataSet
{
    using System;

    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.DataSet.Fox.FoxGameKit;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;

    using UnityEngine;

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
            }
        }

        public DataSetAsset Asset
        {
            set
            {
                this.asset = value;
            }
        }

        void Update()
        {
            //this.gameObject.SetActive(this.entity.Visibility);

            if (this.transform.parent != null)
            {
                var parentSceneProxy = this.transform.parent.GetComponent<SceneProxy>();
                if (parentSceneProxy == null)
                {
                    return;
                }

                this.entity.Parent = parentSceneProxy.entity;
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
            Gizmos.DrawLine(this.transform.position + Vector3.back, this.transform.position + Vector3.forward);
            Gizmos.DrawLine(this.transform.position + Vector3.down, this.transform.position + Vector3.up);
            Gizmos.DrawLine(this.transform.position + Vector3.left, this.transform.position + Vector3.right);
        }

        private Vector3 previousEntityTranslation;

        private Quaternion previousEntityRotation;

        private Vector3 previousEntityScale;

        void OnDrawGizmosSelected()
        {
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
                Gizmos.DrawLine(this.transform.position + Vector3.back, this.transform.position + Vector3.forward);
                Gizmos.DrawLine(this.transform.position + Vector3.down, this.transform.position + Vector3.up);
                Gizmos.DrawLine(this.transform.position + Vector3.left, this.transform.position + Vector3.right);
            }

            EditorUtility.SetDirty(this.asset);
        }
    }
}