namespace FoxKit.Modules.DataSet
{
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// A sub-entity of a SceneProxy, such as a model.
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class SceneProxyChild : MonoBehaviour
    {
        public SceneProxy Owner;

        void Update()
        {
            if (this.transform.hasChanged)
            {
                this.Owner.transform.position = this.transform.position;
                this.transform.localPosition = Vector3.zero;
                this.transform.hasChanged = false;
            }
        }
    }
}