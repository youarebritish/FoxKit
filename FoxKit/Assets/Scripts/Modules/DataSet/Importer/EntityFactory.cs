using FoxKit.Modules.DataSet.FoxCore;
using FoxTool.Fox;
using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Importer
{
    public static class EntityFactory
    {
        public delegate Type GetEntityTypeDelegate(string className);
        public delegate Entity GetEntityFromAddressDelegate(ulong address);
        
        public static Entity Create(FoxEntity data, GetEntityTypeDelegate getEntityType)
        {
            var type = getEntityType(data.ClassName);
            if (type == null)
            {
                return null;
            }

            var instance = ScriptableObject.CreateInstance(type) as Entity;
            return instance;
        }
    }
}