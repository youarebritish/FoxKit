namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;
    
    /// <inheritdoc />
    /// <summary>
    /// Base class for Fox Engine objects.
    /// </summary>
    [Serializable]
    public abstract class Entity : ScriptableObject
    {
        /// <summary>
        /// Gets the parent of this Entity, if any.
        /// </summary>
        public virtual Entity Parent => this.DataSet;

        /// <summary>
        /// Gets the children of this Entity, if any.
        /// </summary>
        /// <returns>
        /// The Entity's children.
        /// </returns>
        public virtual IEnumerable<Entity> Children => new List<Entity>();

        /// <summary>
        /// The icon to use in the Data List window.
        /// </summary>
        public virtual Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image as Texture2D;

        /// <summary>
        /// ID of the class.
        /// </summary>
        public virtual short ClassId => -1;

        /// <summary>
        /// Version of the class.
        /// </summary>
        public virtual ushort Version => 0;

        /// <summary>
        /// Gets or sets the DataSet this Entity belongs to.
        /// </summary>
        protected DataSet DataSet { get; set; }

        /// <summary>
        /// Initializes the Entity with data loaded from a DataSet file.
        /// </summary>
        /// <param name="dataSet">
        /// The DataSet which owns this Entity.
        /// </param>
        /// <param name="entityData">
        /// The data loaded from a DataSet file.
        /// </param>
        /// <param name="initFunctions">
        /// Helper functions to aid in initialization.
        /// </param>
        public void Initialize(DataSet dataSet, Core.Entity entityData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            Assert.AreEqual(entityData.ClassId, this.ClassId, $"Expected ID {this.ClassId} for class {entityData.ClassName}, but was {entityData.ClassId}.");
            Assert.AreEqual(entityData.Version, this.Version, $"Expected version {this.Version} for class {entityData.ClassName}, but was {entityData.Version}.");
            
            foreach (var property in entityData.StaticProperties)
            {
                this.ReadProperty(property, initFunctions);
            }

            foreach (var unused in entityData.DynamicProperties)
            {
                Debug.LogError($"Attempted to read dynamic property in an entity of type {entityData.ClassName} but dynamic properties are not yet supported.");
            }
        }

        /// <summary>
        /// This is called after importing of any number of assets is complete (when the Assets progress bar has reached the end).
        /// </summary>
        /// <param name="tryGetAsset">
        /// Function to load a newly-imported or already existing asset.
        /// </param>
        public virtual void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
        }

        /// <summary>
        /// Invoked when the containing DataSet is loaded.
        /// </summary>
        public virtual void OnLoaded()
        {
        }

        /// <summary>
        /// Invoked for all Entities in a DataSet after OnLoaded() has been called for each of them.
        /// </summary>
        public virtual void PostOnLoaded()
        {
        }


        /// <summary>
        /// Invoked when the containing DataSet is unloaded.
        /// </summary>
        public virtual void OnUnloaded()
        {
        }

        /// <summary>
        /// Creates writable list of Entity static properties.
        /// </summary>
        /// <param name="getEntityAddress">
        /// Function to get an Entity's address.
        /// </param>
        /// <returns>
        /// Writable static properties.
        /// </returns>
        public abstract List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress);

        /// <summary>
        /// Creates writable list of Entity dynamic properties.
        /// </summary>
        /// <param name="getEntityAddress">
        /// Function to get an Entity's address.
        /// </param>
        /// <returns>
        /// Writable dynamic properties.
        /// </returns>
        public virtual List<Core.PropertyInfo> MakeWritableDynamicProperties(Func<Entity, ulong> getEntityAddress)
        {
            return new List<Core.PropertyInfo>();
        }

        /// <summary>
        /// Initializes a property from data loaded from a DataSet file.
        /// </summary>
        /// <param name="propertyData">
        /// The property data.
        /// </param>
        /// <param name="initFunctions">
        /// Initialization functions.
        /// </param>
        protected virtual void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
        }
    }
}