namespace FoxKit.Modules.DataSet.Importer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;
    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;
    using UnityEngine.Assertions;

    using static FoxKit.Modules.DataSet.Importer.EntityFactory;

    /// <inheritdoc />
    /// <summary>
    /// Imports DataSets.
    /// </summary>
    [ScriptedImporter(1, new[] { "fox2", "parts", "bnd", "clo", "des", "evf", "fsd", "lad", "ph", "phsd", "sdf", "sim", "tgt", "vdp", "veh", "vfxlf" })]
    public class DataSetImporter : ScriptedImporter
    {
        /// <summary>
        /// All subclasses of Entity.
        /// TODO: Cache this somewhere.
        /// </summary>
        public static readonly Type[] EntityTypes = ReflectionUtils.GetAssignableConcreteClasses(typeof(Entity)).ToArray();
        
        /// <inheritdoc />
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var asset = CreateAsset(Path.GetExtension(ctx.assetPath));
            asset.IsReadOnly = true;
            Assert.IsNotNull(ctx.assetPath);

            asset.name = Path.GetFileNameWithoutExtension(ctx.assetPath);
            
            IEnumerable<Core.Entity> rawEntities;
            using (var input = new FileStream(ctx.assetPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(input))
                {
                    rawEntities = DataSetFile2.Read(MakeReadFunctions(reader));
                }
            }
            
            var entities = CreateEntityInstances(rawEntities, Path.GetFileName(ctx.assetPath), MakeEntityCreateFunctions());
            var dataSet = FindDataSet(entities);
            dataSet.OwningDataSetName = asset.name;

            var dataSetGuid = AssetDatabase.AssetPathToGUID(this.assetPath);

            InitializeEntities(entities, dataSetGuid, MakeEntityInitializeFunctions(dataSet, entities));

            asset.SetDataSet(dataSet);

            ctx.AddObjectToAsset("DataSet", asset);
            ctx.SetMainObject(asset);
        }

        private static EntityFileAsset CreateAsset(string extension)
        {
            var typeToCreate = typeof(DataSetAsset);
            switch (extension)
            {
                case ".fox2":
                    break;
                case ".bnd":
                    typeToCreate = typeof(BounderFileAsset);
                    break;
                case ".clo":
                    typeToCreate = typeof(ClothSettingFileAsset);
                    break;
                case ".des":
                    typeToCreate = typeof(DestructionFileAsset);
                    break;
                case ".evf":
                    typeToCreate = typeof(EventFileAsset);
                    break;
                case ".fsd":
                    typeToCreate = typeof(FacialSettingFileAsset);
                    break;
                case ".parts":
                    typeToCreate = typeof(PartsFileAsset);
                    break;
                case ".ph":
                    typeToCreate = typeof(PhysicsFileAsset);
                    break;
                case ".phsd":
                    typeToCreate = typeof(SoundFileAsset);
                    break;
                case ".sdf":
                    typeToCreate = typeof(SoundDataFileAsset);
                    break;
                case ".sim":
                    typeToCreate = typeof(SimFileAsset);
                    break;
                case ".tgt":
                    typeToCreate = typeof(TargetFileAsset);
                    break;
                case ".veh":
                    typeToCreate = typeof(VehicleFileAsset);
                    break;
                case ".vfxlf":
                    typeToCreate = typeof(LensFlareFileAsset);
                    break;
                default:
                    Debug.LogWarning($"Unsupported DataSetFile2 extension: {extension}");
                    break;
            }

            var asset = ScriptableObject.CreateInstance(typeToCreate) as EntityFileAsset;
            return asset;
        }

        private static DataSetFile2.ReadFunctions MakeReadFunctions(BinaryReader reader)
        {
            return new DataSetFile2.ReadFunctions(
                reader.ReadSByte,
                reader.ReadByte,
                reader.ReadInt16,
                reader.ReadUInt16,
                reader.ReadInt32,
                reader.ReadUInt32,
                reader.ReadInt64,
                reader.ReadUInt64,
                reader.ReadSingle,
                reader.ReadDouble,
                reader.ReadBoolean,
                length => new string(reader.ReadChars((int)length)),
                () => reader.BaseStream.Position,
                newPosition => reader.BaseStream.Position = newPosition,
                delegate(int numBytes) { reader.ReadBytes(numBytes); },
                    alignment => AlignRead(reader.BaseStream, alignment));
        }

        private static void AlignRead(Stream stream, long alignment)
        {
            var alignmentRequired = stream.Position % alignment;
            if (alignmentRequired > 0)
            {
                stream.Position = stream.Position + alignment - alignmentRequired;
            }
        }

        /// <summary>
        /// Creates FoxKit instances of the imported entities.
        /// </summary>
        /// <param name="foxFile">
        /// The imported DataSet file.
        /// </param>
        /// <param name="entityCreateFunctions">
        /// Helper functions to initialize an entity.
        /// </param>
        /// <returns>
        /// The created Entity instances.
        /// </returns>
        private static Dictionary<Entity, Core.Entity> CreateEntityInstances(IEnumerable<Core.Entity> rawEntities, string filename, EntityCreateFunctions entityCreateFunctions)
        {
            return (from entity in rawEntities
                    select new { Data = entity, Instance = Create(entity, filename, entityCreateFunctions) })
                .Where(entry => entry.Instance != null)
                .ToDictionary(entry => entry.Instance, entry => entry.Data);
        }

        /// <summary>
        /// Find the DataSet instance.
        /// </summary>
        /// <param name="entities">
        /// All loaded entities.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/> entity instance.
        /// </returns>
        private static DataSet FindDataSet(Dictionary<Entity, Core.Entity> entities)
        {
            return (from entity in entities.Keys
                    where entity.GetType() == typeof(DataSet)
                    select entity as DataSet)
                .First();
        }

        /// <summary>
        /// Initialize newly-created entity instances.
        /// </summary>
        /// <param name="entities">
        /// The entities to initialize.
        /// </param>
        /// <param name="entityInitializeFunctions">
        /// The entity initialize functions.
        /// </param>
        private static void InitializeEntities(
            Dictionary<Entity, Core.Entity> entities,
            string dataSetGuid,
            EntityInitializeFunctions entityInitializeFunctions)
        {
            foreach (var entity in entities)
            {
                entity.Key.Initialize(entity.Value, entityInitializeFunctions);

                // TODO: Make this less ugly
                var data = entity.Key as Data;
                if (data != null)
                {
                    data.DataSetGuid = dataSetGuid;
                }

                // We don't need to add the DataSet to itself, and it gets added to the asset later.
                if (entity.Key.GetType() == typeof(DataSet) || (entity.Key as Data) == null)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Create the Entity factory functions.
        /// </summary>
        /// <returns>
        /// The <see cref="EntityCreateFunctions"/>.
        /// </returns>
        private static EntityCreateFunctions MakeEntityCreateFunctions()
        {
            return new EntityCreateFunctions(GetEntityType);
        }

        /// <summary>
        /// Create the Entity initialization functions.
        /// </summary>
        /// <param name="entities">
        /// The entities to initialize.
        /// </param>
        /// <returns>
        /// The <see cref="EntityInitializeFunctions"/>.
        /// </returns>
        private static EntityInitializeFunctions MakeEntityInitializeFunctions(DataSet dataSet, Dictionary<Entity, Core.Entity> entities)
        {
            EntityInitializeFunctions.GetEntityFromAddressDelegate getEntityByAddress =
                address => entities.FirstOrDefault(e => e.Value.Address == address).Key;

            Func<string, Data> getEntityByName = dataSet.GetData;

            return new EntityInitializeFunctions(
                getEntityByAddress,
                entityLink => DataSetUtils.MakeEntityLink(dataSet, entityLink, getEntityByAddress, getEntityByName));
        }

        /// <summary>
        /// Get the type of an Entity.
        /// </summary>
        /// <param name="className">
        /// The class name of the Entity.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        private static Type GetEntityType(string className)
        {
            foreach (var type in EntityTypes)
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