using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Modules.DataSet.GameCore;
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
using static FoxKit.Modules.DataSet.Importer.EntityFactory;

namespace FoxKit.Modules.DataSet.Importer
{
    [ScriptedImporter(1, "fox2")]
    public class DataSetImporter : ScriptedImporter
    {
        // TODO: Remove/cache
        private static readonly Type[] typesInAddMenu = ReflectionUtils.GetAssignableConcreteClasses(typeof(Entity)).ToArray();
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

            var entityCreateFunctions = MakeEntityCreateFunctions();

            var entities = (from entity in foxFile.Entities
                            select new { Data = entity, Instance = Create(entity, entityCreateFunctions) })
                            .Where(entry => entry.Instance != null)
                            .ToDictionary(entry => entry.Instance, entry => entry.Data);

            // Find DataSet.
            FoxCore.DataSet dataSet = null;
            foreach(var entity in entities.Keys)
            {
                if (entity.GetType() == typeof(FoxCore.DataSet))
                {
                    dataSet = entity as FoxCore.DataSet;
                    break;
                }                
            }

            var entityInitializeFunctions = MakeEntityInitializeFunctions(entities);
            foreach (var entity in entities)
            {
                
                entity.Key.Initialize(entity.Value, entityInitializeFunctions);

                // TODO Fix null entries
                if (entity.Key.GetType() == typeof(FoxCore.DataSet))
                {
                    continue;
                }

                // TODO Make this more elegant
                ctx.AddObjectToAsset(entity.Key.name, entity.Key);
                if (!string.IsNullOrEmpty(entity.Key.name))
                {                    
                    dataSet.DataList.Add(entity.Key.name, entity.Key);
                }
            }

            ctx.AddObjectToAsset("DataSet", dataSet);
            ctx.SetMainObject(dataSet);
        }

        private static EntityCreateFunctions MakeEntityCreateFunctions()
        {
            return new EntityCreateFunctions(GetEntityType);
        }

        private static EntityInitializeFunctions MakeEntityInitializeFunctions(Dictionary<Entity, FoxEntity> entities)
        {
            return new EntityInitializeFunctions((address) => entities.FirstOrDefault(e => e.Value.Address == address).Key);
        }

        private static Type GetEntityType(string className)
        {
            foreach (var type in typesInAddMenu)
            {
                if (type.Name == className)
                {
                    return type;
                }
            }
            Debug.LogError("Unable to find class " + className);
            return null;
        }
    }
}