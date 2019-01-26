namespace FoxKit.Modules.DataSet.Exporter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEngine.Assertions;

    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Collection of helper functions for exporting Entities to DataSetFile2 format.
    /// </summary>
    public static class DataSetExporter
    {
        // TODO Refactor out
        private static Dictionary<Entity, uint> entityIds = new Dictionary<Entity, uint>();

        public static void ExportDataSet(List<Entity> entities, string exportPath)
        {
            Assert.IsNotNull(exportPath, "exportPath must not be null.");

            Func<uint> generateId = new IdGenerator().Next;
            entityIds = new Dictionary<Entity, uint>();
            foreach (var entity in entities)
            {
                entityIds.Add(entity, generateId());
            }
            
            var convertedEntities = ConvertEntities(entities, GetEntityAddressAndId).ToList();
            using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create)))
            {
                DataSetFile2.Write(convertedEntities, CreateWriteFunctions(writer));
            }
        }

        private static Tuple<uint, uint> GetEntityAddressAndId(Entity entity)
        {
            if (entity == null)
            {
                return Tuple.Create(0u, 0u);
            }

            uint id;
            entityIds.TryGetValue(entity, out id);
            return Tuple.Create(entity.Address, id);
        }

        private static IEnumerable<Core.Entity> ConvertEntities(IEnumerable<Entity> entities, Func<Entity, Tuple<uint, uint>> getEntityAddressAndId)
        {
            return from entity in entities
                   select ConvertEntity(entity, getEntityAddressAndId(entity).Item1, getEntityAddressAndId(entity).Item2, entity1 => getEntityAddressAndId(entity1).Item1);
        }

        private static Core.Entity ConvertEntity(Entity entity, uint address, uint id, Func<Entity, ulong> getEntityAddress)
        {
            entity.OnPreparingToExport();

            return new Core.Entity(
                entity.GetType().Name,
                address,
                id,
                entity.ClassId,
                entity.Version,
                entity.MakeWritableStaticProperties(getEntityAddress, DataSetUtils.MakeEntityLink).ToArray(),
                entity.MakeWritableDynamicProperties(getEntityAddress).ToArray());
        }

        private static DataSetFile2.WriteFunctions CreateWriteFunctions(BinaryWriter writer)
        {
            return new DataSetFile2.WriteFunctions(
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
                writer.Write,
            () => writer.BaseStream.Position,
            position => writer.BaseStream.Position = position,
            numZeroes => WriteZeroes(writer, numZeroes),
            (alignment, data) => AlignWrite(writer.BaseStream, alignment));
        }

        private static void AlignWrite(Stream stream, int alignment)
        {
            var alignmentRequired = stream.Position % alignment;
            if (alignmentRequired <= 0)
            {
                return;
            }

            var alignmentBytes = Enumerable.Repeat((byte)0, (int)(alignment - alignmentRequired)).ToArray();
            stream.Write(alignmentBytes, 0, alignmentBytes.Length);
        }

        private static void WriteZeroes(BinaryWriter writer, uint numZeroes)
        {
            var zeroes = new byte[numZeroes];
            writer.Write(zeroes);
        }

        private class IdGenerator
        {
            private uint previousId = 0u;

            public uint Next()
            {
                this.previousId++;
                return this.previousId;
            }
        }
    }
}