namespace FoxKit.Modules.DataSet.FoxCore
{
    using FoxTool.Fox;
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class Entity : ScriptableObject
    {
        public void Initialize(FoxEntity entityData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            // TODO: Assert that class ID and version is correct
            foreach(var property in entityData.StaticProperties)
            {
                ReadProperty(property, initFunctions);
            }
            foreach(var property in entityData.DynamicProperties)
            {
                Debug.LogError($"Attempted to read dynamic property in an entity of type {entityData.ClassName} but dynamic properties are not yet supported.");
            }
        }

        protected virtual void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
        }

        public virtual void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
        }
    }
}