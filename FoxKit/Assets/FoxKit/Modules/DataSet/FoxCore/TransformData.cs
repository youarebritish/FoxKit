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
    public abstract class TransformData : Data
    {
        /// <summary>
        /// The transform matrix.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Transform", PropertyAttribute.NestedInspectorMode.Draw)]
        protected TransformEntity transform;

        /// <summary>
        /// The TransformData, if any, to which this one belongs.
        /// </summary>
        [SerializeField, HideInInspector]
        private TransformData parent;

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
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress);
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "parent",
                    Core.PropertyInfoType.EntityHandle,
                    getEntityAddress(this.parent)));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "transform",
                    Core.PropertyInfoType.EntityPtr,
                    getEntityAddress(this.transform)));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "shearTransform",
                    Core.PropertyInfoType.EntityPtr,
                    getEntityAddress(this.transform)));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty(
                    "pivotTransform",
                    Core.PropertyInfoType.EntityPtr,
                    getEntityAddress(this.transform)));
            parentProperties.Add(
                PropertyInfoFactory.MakeListProperty(
                    "children",
                    Core.PropertyInfoType.EntityPtr,
                    (from child in this.children select getEntityAddress(child) as object).ToArray()));
            var packedFlags = (uint)((this.visibility ? Flags.EnableVisibility : 0)
                                     | (this.selection ? Flags.EnableSelection : 0)
                                     | (this.inheritTransform ? Flags.EnableInheritTransform : 0));
            parentProperties.Add(
                PropertyInfoFactory.MakeStaticArrayProperty("flags", Core.PropertyInfoType.UInt32, packedFlags));

            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "parent":
                    {
                        var address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                        this.parent = initFunctions.GetEntityFromAddress(address) as TransformData;
                        break;
                    }

                case "transform":
                    {
                        var address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                        this.transform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                        if (this.transform != null)
                        {
                            this.transform.Owner = this;
                        }

                        break;
                    }

                case "shearTransform":
                    {
                        var address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                        this.shearTransform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                        if (this.shearTransform != null)
                        {
                            this.shearTransform.Owner = this;
                        }

                        break;
                    }

                case "pivotTransform":
                    {
                        var address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                        this.pivotTransform = initFunctions.GetEntityFromAddress(address) as TransformEntity;

                        if (this.pivotTransform != null)
                        {
                            this.pivotTransform.Owner = this;
                        }

                        break;
                    }

                case "children":
                    this.children = (from handle in DataSetUtils.GetListValues<ulong>(propertyData)
                                     select initFunctions.GetEntityFromAddress(handle) as TransformData)
                                     .ToList();
                    break;

                case "flags":
                    var flags = (Flags)DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData);
                    this.inheritTransform = flags.HasFlag(Flags.EnableInheritTransform);
                    this.visibility = flags.HasFlag(Flags.EnableVisibility);
                    this.selection = flags.HasFlag(Flags.EnableVisibility);
                    break;
            }
        }
    }
}