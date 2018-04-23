using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Containers;
using FoxTool.Fox.Types.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Importer
{
    [ScriptedImporter(1, "fox2")]
    public class DataSetImporter : ScriptedImporter
    {
        // TODO: Remove/cache
        private static readonly Type[] typesInAddMenu = ReflectionUtils.GetAssignableConcreteClasses(typeof(Data)).ToArray();

        private static readonly Dictionary<ulong, string> GlobalHashNameDictionary = new Dictionary<ulong, string>();

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var asset = ScriptableObject.CreateInstance<FoxCore.DataSet>();
            asset.name = Path.GetFileNameWithoutExtension(ctx.assetPath);

            var fileName = string.Format("{0}.xml", Path.GetFileName(asset.name));
            using (var input = new FileStream(ctx.assetPath, FileMode.Open))
            {
                var lookupTable = new FoxLookupTable(GlobalHashNameDictionary);
                var foxFile = FoxFile.ReadFoxFile(input, lookupTable);

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
                    var nameProperty = entity.StaticProperties.First(property => property.Name == "name");
                    var container = (nameProperty.Container as FoxListBase<FoxString>);
                    instance.name = container.ToList()[0].ToString();

                    entities.Add(instance);
                }

                FoxCore.DataSet dataSet = null;
                foreach(var entity in entities)
                {
                    if (entity.GetType() == typeof(FoxCore.DataSet))
                    {
                        dataSet = entity as FoxCore.DataSet;
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
                    //AssetDatabase.AddObjectToAsset(entity, dataSet);
                    //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(entity));
                    dataSet.DataList.Add(entity.name, entity);
                }
            }
        }
    }
}