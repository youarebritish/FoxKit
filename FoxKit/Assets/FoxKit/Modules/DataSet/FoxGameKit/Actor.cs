namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using UnityEngine;

    public class Actor : Entity
    {
        private string name;

        [SerializeField, Modules.DataSet.Property("Actor")]
        private float actorLevelName;

        [SerializeField, Modules.DataSet.Property("Actor")]
        private float sceneName;
        
        public void AddToScene()
        {
            throw new NotImplementedException();
        }
    }
}