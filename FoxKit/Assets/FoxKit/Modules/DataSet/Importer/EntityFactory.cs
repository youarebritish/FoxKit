using FoxKit.Modules.DataSet.FoxCore;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Importer
{
    public static class EntityFactory
    {        
        public static Entity Create(FoxEntity data, EntityCreateFunctions createFunctions)
        {
            var type = createFunctions.GetEntityType(data.ClassName);
            if (type == null)
            {
                return null;
            }

            var instance = ScriptableObject.CreateInstance(type) as Entity;
            return instance;
        }

        public class EntityCreateFunctions
        {
            public delegate Type GetEntityTypeDelegate(string className);
            public GetEntityTypeDelegate GetEntityType { get; }

            public EntityCreateFunctions(GetEntityTypeDelegate getEntityType)
            {
                this.GetEntityType = getEntityType;
            }
        }

        public class EntityInitializeFunctions
        {
            public delegate Entity GetEntityFromAddressDelegate(ulong address);
            public delegate void DeliverRequestedFileDelegate(UnityEngine.Object requestedFile);
            public delegate void RequestFileDelegate(string requestedFilePath, int requestingAssetPath);

            public GetEntityFromAddressDelegate GetEntityFromAddress { get; }
            public RequestFileDelegate RequestFile { get; }

            public EntityInitializeFunctions(GetEntityFromAddressDelegate getEntityFromAddress, RequestFileDelegate requestFile)
            {
                this.GetEntityFromAddress = getEntityFromAddress;
                this.RequestFile = requestFile;
            }
        }
    }
}