namespace FoxKit.Modules.DataSet.Exporter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using UnityEngine.Assertions;

    /// <summary>
    /// Collection of helper functions for exporting Entities to DataSetFile2 format.
    /// </summary>
    public static class DataSetExporter
    {
        private static Dictionary<Entity, Tuple<uint, uint>> entityAddressesAndIds = new Dictionary<Entity, Tuple<uint, uint>>();

        public static void ExportDataSet(List<Entity> entities, string exportPath)
        {
            Assert.IsNotNull(exportPath, "exportPath must not be null.");

            Func<Tuple<uint, uint>> generateAddressAndId = new ClassAddressAndIdGenerator().Next;
            /*var entityAddressesAndIds = entities.ToDictionary(
                entity => entity,
                entity => generateAddressAndId());*/

            entityAddressesAndIds = new Dictionary<Entity, Tuple<uint, uint>>();
            foreach (var entity in entities)
            {
                entityAddressesAndIds.Add(entity, generateAddressAndId());
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

            Tuple<uint, uint> record;
            if (entityAddressesAndIds.TryGetValue(entity, out record))
            {
                return record;
            }
            return Tuple.Create(0u, 0u);
        }

        private static IEnumerable<Core.Entity> ConvertEntities(IEnumerable<Entity> entities, Func<Entity, Tuple<uint, uint>> getEntityAddressAndId)
        {
            return from entity in entities
                   select ConvertEntity(entity, getEntityAddressAndId(entity).Item1, getEntityAddressAndId(entity).Item2, entity1 => getEntityAddressAndId(entity1).Item1);
        }

        private static Core.Entity ConvertEntity(Entity entity, uint address, uint id, Func<Entity, ulong> getEntityAddress)
        {
            return new Core.Entity(
                entity.GetType().Name,
                address,
                id,
                entity.ClassId,
                entity.Version,
                entity.MakeWritableStaticProperties(getEntityAddress).ToArray(),
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

        private class ClassAddressAndIdGenerator
        {
            private uint previousAddress = 0x10000000u;
            private uint previousId = 0u;

            public Tuple<uint, uint> Next()
            {
                this.previousAddress += 0x70;
                this.previousId++;

                return Tuple.Create(this.previousAddress, this.previousId);
            }
        }
    }
}