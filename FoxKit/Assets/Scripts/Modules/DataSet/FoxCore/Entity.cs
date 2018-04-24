namespace FoxKit.Modules.DataSet.FoxCore
{
    using FoxTool.Fox;
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class Entity : ScriptableObject
    {
        public void Initialize(FoxEntity entityData)
        {
            // TODO: Assert that class ID and version is correct
            foreach(var property in entityData.StaticProperties)
            {
                ReadProperty(property);
            }
            foreach(var property in entityData.DynamicProperties)
            {
                Debug.LogError($"Attempted to read dynamic property in an entity of type {entityData.ClassName} but dynamic properties are not yet supported.");
            }
        }

        protected virtual void ReadProperty(FoxProperty propertyData)
        {
        }
    }
}