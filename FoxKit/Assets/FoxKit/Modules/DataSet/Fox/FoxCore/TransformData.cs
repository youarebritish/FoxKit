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

    public partial class TransformData
    {
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

        public bool Visibility
        {
            get
            {
                return this.flags.HasFlag(TransformData_Flags.EnableVisibility);
            }

            set
            {
                if (value == true)
                {
                    this.flags |= TransformData_Flags.EnableVisibility;
                }
                else
                {
                    this.flags &= ~TransformData_Flags.EnableVisibility;
                }

                this.visibility = value;
            }
        }

        public bool Selection
        {
            get
            {
                return this.flags.HasFlag(TransformData_Flags.EnableSelection);
            }

            set
            {
                if (value == true)
                {
                    this.flags |= TransformData_Flags.EnableSelection;
                }
                else
                {
                    this.flags &= ~TransformData_Flags.EnableSelection;
                }

                this.selection = value;
            }
        }

        public TransformData Parent
        {
            get
            {
                return this.parent as TransformData;
            }
            set
            {
                if (this.parent != value)
                {
                    value.children.Add(this);

                    (this.parent as TransformData)?.children.Remove(this);
                }

                this.parent = value;
            }
        }
        
        public IEnumerable<TransformData> GetChildren()
        {
            return from child in this.children
                   select child as TransformData;
        }

        public TransformData()
            : base()
        {
            this.Visibility = true;
            this.Selection = true;
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

                if (childSceneProxy == null)
                {
                    continue;
                }

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