using System;
using System.IO;
using System.Runtime.InteropServices;
using FtexTool.Exceptions;

namespace FtexTool
{
    internal static class ExtensionMethods
    {
        internal static void Skip(this BinaryReader reader, int count)
        {
            reader.BaseStream.Seek(count, SeekOrigin.Current);
        }

        internal static void WriteZeros(this BinaryWriter writer, int count)
        {
            byte[] zeros = new byte[count];
            writer.Write(zeros);
        }

        internal static void Assert<T>(this BinaryReader reader, T expected, string message = "") where T : struct
        {
            T actual = ReadValue<T>(reader);
            if (actual.Equals(expected) == false)
                throw new AssertionFailedException(message);
        }

        private static T ReadValue<T>(BinaryReader reader) where T : struct
        {
            int size = SizeOf(typeof(T));
            byte[] data = reader.ReadBytes(size);
            T actual = ByteArrayToStructure<T>(data);
            return actual;
        }

        internal static int Align(this BinaryWriter writer, int alignment)
        {
            int alignmentRequired = (int)(writer.BaseStream.Position % alignment);
            if (alignmentRequired > 0)
            {
                int bytesToAdd = alignment - alignmentRequired;
                writer.BaseStream.Position += bytesToAdd;
                return bytesToAdd;
            }

            return 0;
        }

        internal static int SizeOf(this Type type)
        {
            return Marshal.SizeOf(type);
        }

        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;
        }
    }
}
