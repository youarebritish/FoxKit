namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEngine;
    using UnityEngine.Assertions;

    using PropertyAttribute = FoxKit.Modules.DataSet.PropertyAttribute;

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
    public class TransformData : Data
    {
        /// <summary>
        /// The TransformData, if any, to which this one belongs.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityHandle, 120, writable: PropertyExport.EditorOnly)]
        private TransformData parent;

        /// <summary>
        /// The transform matrix.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityPtr, 128, ptrType: typeof(TransformEntity))]
        private TransformEntity transform;

        /// <summary>
        /// The shear transform matrix.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityPtr, 136, ptrType: typeof(ShearTransformEntity))]
        private TransformEntity shearTransform;

        /// <summary>
        /// The pivot transform matrix.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityPtr, 144, ptrType: typeof(TransformEntity))]
        private TransformEntity pivotTransform;

        /// <summary>
        /// The TransformData Entities, if any, which belong to this one.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityHandle, 152, container: Core.ContainerType.List)]
        private List<Entity> children = new List<Entity>();

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 184)]
        private uint flags;

        /// <summary>
        /// Unknown. Believed to be a flag for whether or not this TransformData should inherit its owner's transform.
        /// </summary>
        [SerializeField, Property("Flags")]
        protected bool inheritTransform = true;

        /// <summary>
        /// Whether or not to render this TransformData.
        /// </summary>
        [SerializeField, Property("Flags")]
        protected bool visibility = true;

        /// <summary>
        /// Unknown. Believed to be a flag for whether or not this TransformData should be selectable in the editor.
        /// </summary>
        [SerializeField, Property("Flags")]
        protected bool selection = true;

        public bool Visibility => this.visibility;

        public bool InheritTransform => this.inheritTransform;

        public bool Selection => this.selection;
        
        public TransformData Parent => this.parent;

        public TransformEntity Transform
        {
            get
            {
                return this.transform;
            }
            set
            {
                this.transform = value;
            }
        }
        
        public IEnumerable<TransformData> GetChildren()
        {
            return from child in this.children
                   select child as TransformData;
        }

        /// <inheritdoc />
        public override void OnLoaded(CreateSceneProxyDelegate createSceneProxy)
        {
            base.OnLoaded(createSceneProxy);
            this.CreateSceneProxy(createSceneProxy);
        }

        /// <inheritdoc />
        public override void PostOnLoaded(GetSceneProxyDelegate getSceneProxy)
        {
            base.PostOnLoaded(getSceneProxy);
            
            foreach (var child in this.GetChildren())
            {
                // A child is occasionally (but very rarely) null. Seems to only happen in ombs.
                if (child == null)
                {
                    continue;
                }

                var sceneProxy = getSceneProxy(this.Name);
                var childSceneProxy = getSceneProxy(child.Name);

                childSceneProxy.transform.SetParent(sceneProxy.transform);
            }
        }

        /// <inheritdoc />
        public override void OnUnloaded(DestroySceneProxyDelegate destroySceneProxy)
        {
            base.OnUnloaded(destroySceneProxy);
            this.DestroySceneProxy(destroySceneProxy);
        }
        
        protected virtual void CreateSceneProxy(CreateSceneProxyDelegate createSceneProxy)
        {
            if (this.transform == null)
            {
                return;
            }

            var sceneProxy = createSceneProxy();

            // TODO: Rotation et al
            sceneProxy.transform.position = this.transform.Translation;
        }

        protected virtual void DestroySceneProxy(DestroySceneProxyDelegate destroySceneProxy)
        {
            destroySceneProxy();
        }
    }
}