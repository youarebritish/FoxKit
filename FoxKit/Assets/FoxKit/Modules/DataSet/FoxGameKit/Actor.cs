namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxLib;

    using UnityEngine;

    public class Actor : Entity
    {
        /// <summary>
        /// Just use the ScriptableObject's name for now.
        /// </summary>
        public virtual string Name => this.name;

        [SerializeField, Modules.DataSet.Property("Actor")]
        private float actorLevelName;

        [SerializeField, Modules.DataSet.Property("Actor")]
        private float sceneName;

        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            throw new NotImplementedException("Actors should not be written to file!");
        }

        public void AddToScene()
        {
            throw new NotImplementedException();
        }
    }
}