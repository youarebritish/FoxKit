namespace FoxKit.Modules.DataSet
{
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// A scene representation of a FoxKit Entity. Created to visualize spatial Entity data.
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneProxy : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private TransformData entity;

        [SerializeField, HideInInspector]
        private DataSetAsset asset;

        public TransformData Entity
        {
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

        void OnDrawGizmosSelected()
        {
            // TODO Don't allow to move if read-only.
            this.entity.Transform.Translation = this.transform.position;
            this.entity.Transform.RotQuat = this.transform.rotation;
            this.entity.Transform.Scale = this.transform.localScale;

            EditorUtility.SetDirty(this.asset);
        }
    }
}