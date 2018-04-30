namespace FoxKit.Modules.DataSet.Importer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;

    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;

    using static FoxKit.Modules.DataSet.Importer.EntityFactory;

    [ScriptedImporter(1, new[] { "fox2", "parts" })]
    public class DataSetImporter : ScriptedImporter
    {
        // TODO: Remove/cache
        private static readonly Type[] typesInAddMenu = ReflectionUtils.GetAssignableConcreteClasses(typeof(Entity)).ToArray();
        private static readonly Dictionary<ulong, string> globalHashNameDictionary = new Dictionary<ulong, string>();

        private static readonly Dictionary<string, int> fileRequests = new Dictionary<string, int>();

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var asset = ScriptableObject.CreateInstance<DataSet>();
            asset.name = Path.GetFileNameWithoutExtension(ctx.assetPath);

            FoxFile foxFile;
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
            var dataSet = (from entity in entities.Keys where entity.GetType() == typeof(DataSet) select entity as DataSet).FirstOrDefault();

            var entityInitializeFunctions = MakeEntityInitializeFunctions(entities);
            foreach (var entity in entities)
            {
                
                entity.Key.Initialize(dataSet, entity.Value, entityInitializeFunctions);

                // TODO Fix null entries
                if (entity.Key.GetType() == typeof(DataSet))
                {
                    continue;
                }

                // TODO Make this more elegant
                ctx.AddObjectToAsset(entity.Key.name, entity.Key);
                if (string.IsNullOrEmpty(entity.Key.name))
                {
                    continue;
                }
                if (dataSet == null)
                {
                    continue;
                }
                dataSet.AddData(entity.Key.name, entity.Value.Address, entity.Key);
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
            return new EntityInitializeFunctions((address) => entities.FirstOrDefault(e => e.Value.Address == address).Key, RequestFile);
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
                
        // TODO: Remove
        private static void RequestFile(string filePath, int requestingAssetInstanceId)
        {
            Debug.Log("Requesting file: " + filePath);
            fileRequests.Add(Path.GetFileName(filePath), requestingAssetInstanceId);
        }
    }
}