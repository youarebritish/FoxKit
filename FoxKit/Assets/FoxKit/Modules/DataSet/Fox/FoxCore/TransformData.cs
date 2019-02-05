namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Assertions;

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
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image as Texture2D;

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
                return this.visibility;
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
                return this.selection;
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
                // Unity won't allow us to swap parent/child.
                Assert.IsFalse(value?.Parent == this);

                if (this.parent != value)
                {
                    value?.children.Add(this);

                    var parent = this.parent as TransformData;
                    parent?.children.Remove(this);
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
            this.inheritTransform = true;
            this.Visibility = true;
            this.Selection = true;
            this.flags = (TransformData_Flags)7;
        }

        protected override void OnPropertiesLoaded()
        {
            base.OnPropertiesLoaded();

            this.visibility = this.flags.HasFlag(TransformData_Flags.EnableVisibility);
            this.inheritTransform = this.flags.HasFlag(TransformData_Flags.EnableInheritTransform);
            this.selection = this.flags.HasFlag(TransformData_Flags.EnableSelection);
        }

        public override void OnPreparingToExport()
        {
            base.OnPreparingToExport();
            
            if (this.visibility)
            {
                this.flags |= TransformData_Flags.EnableVisibility;
            }
            else
            {
                this.flags &= ~TransformData_Flags.EnableVisibility;
            }

            if (this.inheritTransform)
            {
                this.flags |= TransformData_Flags.EnableInheritTransform;
            }
            else
            {
                this.flags &= ~TransformData_Flags.EnableInheritTransform;
            }

            if (this.selection)
            {
                this.flags |= TransformData_Flags.EnableSelection;
            }
            else
            {
                this.flags &= ~TransformData_Flags.EnableSelection;
            }
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