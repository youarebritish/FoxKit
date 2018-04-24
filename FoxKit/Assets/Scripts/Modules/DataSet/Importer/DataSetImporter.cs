using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Containers;
using FoxTool.Fox.Types.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Importer
{
    [ScriptedImporter(1, "fox2")]
    public class DataSetImporter : ScriptedImporter
    {
        // TODO: Remove/cache
        private static readonly Type[] typesInAddMenu = ReflectionUtils.GetAssignableConcreteClasses(typeof(Data)).ToArray();
        private static readonly Dictionary<ulong, string> globalHashNameDictionary = new Dictionary<ulong, string>();

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var asset = ScriptableObject.CreateInstance<FoxCore.DataSet>();
            asset.name = Path.GetFileNameWithoutExtension(ctx.assetPath);

            FoxFile foxFile = null;
            using (var input = new FileStream(ctx.assetPath, FileMode.Open))
            {
                var lookupTable = new FoxLookupTable(globalHashNameDictionary);
                foxFile = FoxFile.ReadFoxFile(input, lookupTable);
            }

            var entities = new List<Entity>();
            foreach(var entity in foxFile.Entities)
            {
                Type entityType = null;
                foreach(var type in typesInAddMenu)
                {
                    if (type.Name == entity.ClassName)
                    {
                        entityType = type;
                        break;
                    }
                }
                if (entityType == null)
                {
                    Debug.LogError("Unable to find class " + entity.ClassName);
                    continue;
                }

                var instance = ScriptableObject.CreateInstance(entityType) as Entity;
                instance.Initialize(entity);

                // Temporary hack to deal with nameless entities (DataElements), remove this once parenting works
                if (instance.name == string.Empty)
                {
                    instance.name = instance.GetInstanceID().ToString();
                }

                entities.Add(instance);
            }

            FoxCore.DataSet dataSet = null;
            foreach(var entity in entities)
            {
                if (entity.GetType() == typeof(FoxCore.DataSet))
                {
                    dataSet = entity as FoxCore.DataSet;
                    dataSet.name = Path.GetFileNameWithoutExtension(ctx.assetPath);
                    break;
                }
            }
                
            foreach(var entity in entities)
            {
                if (entity.GetType() == typeof(FoxCore.DataSet))
                {
                    ctx.AddObjectToAsset(entity.name, entity);
                    ctx.SetMainObject(entity);
                    continue;
                }

                ctx.AddObjectToAsset(entity.name, entity);
                dataSet.DataList.Add(entity.name, entity);
            }
        }
    }
}