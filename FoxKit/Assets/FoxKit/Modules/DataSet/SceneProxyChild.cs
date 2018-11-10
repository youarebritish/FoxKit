namespace FoxKit.Modules.DataSet
{
    using System.Linq;

    using FoxKit.Modules.DataSet.Fox.FoxGameKit;

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

        [SerializeField, HideInInspector]
        private string modelName;

        void Update()
        {
            if (this.transform.hasChanged)
            {
                this.Owner.transform.position = this.transform.position;
                this.transform.localPosition = Vector3.zero;
                this.transform.hasChanged = false;
            }
        }

        public void SetModel(UnityEngine.Object model)
        {
            this.modelName = model?.name;
        }

        void OnDrawGizmosSelected()
        {
            if (this.Owner.Entity is StaticModel)
            {
                var model = this.Owner.Entity as StaticModel;
                if (model.ModelFile == null)
                {
                    DestroyImmediate(this.gameObject);
                    return;
                }

                if (model.ModelFile.name != this.modelName)
                {
                    this.modelName = model.ModelFile.name;
                    var newModel = Object.Instantiate(model.ModelFile, this.Owner.gameObject.transform) as GameObject;
                    var newChild = newModel.AddComponent<SceneProxyChild>();
                    newChild.Owner = this.Owner;
                    newChild.SetModel(model.ModelFile);
                    DestroyImmediate(this.gameObject);
                }
            }
        }
    }
}