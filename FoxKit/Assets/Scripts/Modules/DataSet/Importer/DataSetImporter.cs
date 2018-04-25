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
using static FoxKit.Modules.DataSet.Importer.EntityFactory;

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

            var entities = (from entity in foxFile.Entities
                            select new { Data = entity, Instance = Create(entity, GetEntityType) })
                            .Where(entry => entry.Instance != null)
                            .ToDictionary(entry => entry.Instance, entry => entry.Data);


            // Find DataSet.
            FoxCore.DataSet dataSet = null;
            foreach(var entity in entities.Keys)
            {
                if (entity.GetType() == typeof(FoxCore.DataSet))
                {
                    dataSet = entity as FoxCore.DataSet;
                    ctx.AddObjectToAsset("DataSet", entity);
                    ctx.SetMainObject(entity);
                    break;
                }
            }

            foreach (var entity in entities)
            {
                GetEntityFromAddressDelegate getEntityByAddress = (address) => entities.FirstOrDefault(e => e.Value.Address == address).Key;
                entity.Key.Initialize(entity.Value, getEntityByAddress);

                // TODO Fix null entries
                if (entity.Key.GetType() == typeof(FoxCore.DataSet))
                {
                    continue;
                }
                
                // TODO Make this more elegant
                if (!string.IsNullOrEmpty(entity.Key.name))
                {
                    ctx.AddObjectToAsset(entity.GetHashCode().ToString(), entity.Key);
                    dataSet.DataList.Add(entity.Key.name, entity.Key);
                }
            }            
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