namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// Bit flags for TransformData.
    /// </summary>
    [Flags]
    public enum Flags : uint
    {
        /// <summary>
        /// Whether or not the TransformData is visible.
        /// </summary>
        EnableVisibility = 1u,

        /// <summary>
        /// Whether or not the TransformData is selectable.
        /// </summary>
        EnableSelection = 2u,

        /// <summary>
        /// Whether or not the TransformData inherits its transform from its parent.
        /// </summary>
        EnableInheritTransform = 4u
    }

    /// <inheritdoc />
    /// <summary>
    /// Base class for Entities with a physical location in the world.
    /// </summary>
    [Serializable]
    public abstract class TransformData : Data
    {
        /// <summary>
        /// The TransformData, if any, to which this one belongs.
        /// </summary>
        [SerializeField, HideInInspector]
        private TransformData parent;

        /// <summary>
        /// The transform matrix.
        /// </summary>
        [SerializeField, Category("Transform", true)]
        protected TransformEntity transform;

        /// <summary>
        /// The shear transform matrix.
        /// </summary>
        [SerializeField, HideInInspector]
        private TransformEntity shearTransform;

        /// <summary>
        /// The pivot transform matrix.
        /// </summary>
        [SerializeField, HideInInspector]
        private TransformEntity pivotTransform;

        /// <summary>
        /// The TransformData Entities, if any, which belong to this one.
        /// </summary>
        [SerializeField, HideInInspector]
        private List<TransformData> children = new List<TransformData>();

        /// <summary>
        /// Unknown. Believed to be a flag for whether or not this TransformData should inherit its owner's transform.
        /// </summary>
        [SerializeField, Category("Flags")]
        protected bool inheritTransform = true;

        /// <summary>
        /// Whether or not to render this TransformData.
        /// </summary>
        [SerializeField, Category("Flags")]
        protected bool visibility = true;

        /// <summary>
        /// Unknown. Believed to be a flag for whether or not this TransformData should be selectable in the editor.
        /// </summary>
        [SerializeField, Category("Flags")]
        protected bool selection = true;

        [SerializeField, HideInInspector]
        private GameObject sceneProxyGameObject;

        public Transform SceneProxyTransform => this.sceneProxyGameObject.transform;

        /// <inheritdoc />
        public override IEnumerable<Entity> Children => this.children;

        /// <inheritdoc />
        public override Entity Parent => this.parent ?? (Entity)this.DataSet;

        /// <inheritdoc />
        public override void OnLoaded()
        {
            base.OnLoaded();
            this.CreateSceneProxy();
        }

        /// <inheritdoc />
        public override void PostOnLoaded()
        {
            base.PostOnLoaded();
            
            foreach (var child in this.children)
            {
                if (child == null)
                {
                    continue;
                }

                child.SetSceneProxyParent(this.sceneProxyGameObject.transform);
            }
        }

        /// <inheritdoc />
        public override void OnUnloaded()
        {
            base.OnUnloaded();
            this.DestroySceneProxy();
        }
        
        public void SetSceneProxyParent(Transform parent)
        {
            Assert.IsNotNull(this.sceneProxyGameObject);
            this.sceneProxyGameObject.transform.SetParent(parent);
        }

        protected virtual void CreateSceneProxy()
        {
            this.sceneProxyGameObject = new GameObject { name = this.Name };
            this.sceneProxyGameObject.transform.position = this.transform.Translation;
        }

        protected virtual void DestroySceneProxy()
        {
            GameObject.DestroyImmediate(this.sceneProxyGameObject);
            this.sceneProxyGameObject = null;
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "parent":
                    {
                        var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityHandle>(propertyData).Handle;
                        this.parent = initFunctions.GetEntityFromAddress(address) as TransformData;
                        break;
                    }

                case "transform":
                    {
                        var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                        this.transform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                        if (this.transform != null)
                        {
                            this.transform.Owner = this;
                        }

                        break;
                    }

                case "shearTransform":
                    {
                        var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                        this.shearTransform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                        if (this.shearTransform != null)
                        {
                            this.shearTransform.Owner = this;
                        }

                        break;
                    }

                case "pivotTransform":
                    {
                        var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                        this.pivotTransform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                        if (this.pivotTransform != null)
                        {
                            this.pivotTransform.Owner = this;
                        }

                        break;
                    }

                case "children":
                    this.children = (from handle in DataSetUtils.GetListValues<FoxEntityHandle>(propertyData)
                                     select initFunctions.GetEntityFromAddress(handle.Handle) as TransformData)
                                     .ToList();
                    break;

                case "flags":
                    var flags = (Flags)DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
                    this.inheritTransform = flags.HasFlag(Flags.EnableInheritTransform);
                    this.visibility = flags.HasFlag(Flags.EnableVisibility);
                    this.selection = flags.HasFlag(Flags.EnableVisibility);
                    break;
            }
        }
    }
}