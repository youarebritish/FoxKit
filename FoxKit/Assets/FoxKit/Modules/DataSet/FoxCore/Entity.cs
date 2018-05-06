namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxTool.Fox;

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
        /// Gets the DataSet this Entity belongs to.
        /// </summary>
        protected DataSet DataSet { get; private set; }

        /// <summary>
        /// ID of the class.
        /// </summary>
        protected virtual short ClassId => -1;

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
        public void Initialize(DataSet dataSet, FoxEntity entityData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            Assert.AreEqual(entityData.Unknown1, this.ClassId, $"Expected {this.ClassId} for class {entityData.ClassName}, but was {entityData.Unknown2}.");

            this.DataSet = dataSet;
            
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
        public virtual void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
        }

        /// <summary>
        /// Invoked when the containing DataSet is loaded.
        /// </summary>
        public virtual void OnLoaded()
        {
        }

        /// <summary>
        /// Invoked when the containing DataSet is unloaded.
        /// </summary>
        public virtual void OnUnloaded()
        {
        }

        /// <summary>
        /// Gets the children of this Entity, if any.
        /// </summary>
        /// <returns>
        /// The Entity's children.
        /// </returns>
        public virtual IEnumerable<Entity> GetChildren()
        {
            return new List<Entity>();
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
        protected virtual void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
        }
    }
}