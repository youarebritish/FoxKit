namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Bit flags for TransformData.
    /// </summary>
    [Flags]
    public enum TransformData_Flags : uint
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

    public partial class TransformData : Data
    {
        public TransformEntity Transform
        {
            get
            {
                return this.transform as TransformEntity;
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

            sceneProxy.transform.position = this.Transform.Translation;
            sceneProxy.transform.rotation = this.Transform.RotQuat;
            sceneProxy.transform.localScale = this.Transform.Scale;
        }

        protected virtual void DestroySceneProxy(DestroySceneProxyDelegate destroySceneProxy)
        {
            destroySceneProxy();
        }
    }
}