using System;
using System.Globalization;
using System.IO;
using System.Linq;
using FoxTool.Fox.Containers;
using FoxTool.Fox.Types;

namespace FoxTool
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

        internal static void AlignRead(this Stream input, int alignment)
        {
            long alignmentRequired = input.Position % alignment;
            if (alignmentRequired > 0)
                input.Position += alignment - alignmentRequired;
        }

        internal static void AlignWrite(this Stream output, int alignment, byte data)
        {
            long alignmentRequired = output.Position % alignment;
            if (alignmentRequired > 0)
            {
                byte[] alignmentBytes = Enumerable.Repeat(data, (int)(alignment - alignmentRequired)).ToArray();
                output.Write(alignmentBytes, 0, alignmentBytes.Length);
            }
        }

        internal static string ReadString(this BinaryReader binaryReader, int byteCount)
        {
            byte[] bytes = binaryReader.ReadBytes(byteCount);
            return Constants.StringEncoding.GetString(bytes);
        }

        internal static bool IsPrintable(this string s)
        {
            return s.Any(c => char.IsControl(c) || char.IsHighSurrogate(c) || char.IsLowSurrogate(c)) == false;
        }

        internal static FoxContainerType ParseFoxContainerType(string foxContainerType)
        {
            switch (foxContainerType)
            {
                case "StaticArray":
                    return FoxContainerType.StaticArray;
                case "DynamicArray":
                    return FoxContainerType.DynamicArray;
                case "StringMap":
                    return FoxContainerType.StringMap;
                case "List":
                    return FoxContainerType.List;
                default:
                    throw new ArgumentOutOfRangeException("foxContainerType");
            }
        }

        internal static FoxDataType ParseFoxDataType(string foxDataType)
        {
            switch (foxDataType)
            {
                case "int8":
                    return FoxDataType.FoxInt8;
                case "uint8":
                    return FoxDataType.FoxUInt8;
                case "int16":
                    return FoxDataType.FoxInt16;
                case "uint16":
                    return FoxDataType.FoxUInt16;
                case "int32":
                    return FoxDataType.FoxInt32;
                case "uint32":
                    return FoxDataType.FoxUInt32;
                case "int64":
                    return FoxDataType.FoxInt64;
                case "uint64":
                    return FoxDataType.FoxUInt64;
                case "float":
                    return FoxDataType.FoxFloat;
                case "double":
                    return FoxDataType.FoxDouble;
                case "bool":
                    return FoxDataType.FoxBool;
                case "String":
                    return FoxDataType.FoxString;
                case "Path":
                    return FoxDataType.FoxPath;
                case "EntityPtr":
                    return FoxDataType.FoxEntityPtr;
                case "Vector3":
                    return FoxDataType.FoxVector3;
                case "Vector4":
                    return FoxDataType.FoxVector4;
                case "Quat":
                    return FoxDataType.FoxQuat;
                case "Matrix3":
                    return FoxDataType.FoxMatrix3;
                case "Matrix4":
                    return FoxDataType.FoxMatrix4;
                case "Color":
                    return FoxDataType.FoxColor;
                case "FilePtr":
                    return FoxDataType.FoxFilePtr;
                case "EntityHandle":
                    return FoxDataType.FoxEntityHandle;
                case "EntityLink":
                    return FoxDataType.FoxEntityLink;
                case "PropertyInfo":
                    return FoxDataType.FoxPropertyInfo;
                case "WideVector3":
                    return FoxDataType.FoxWideVector3;
                default:
                    throw new ArgumentOutOfRangeException("foxDataType");
            }
        }

        internal static string ToXmlName(this FoxDataType type)
        {
            switch (type)
            {
                case FoxDataType.FoxInt8:
                    return "int8";
                case FoxDataType.FoxUInt8:
                    return "uint8";
                case FoxDataType.FoxInt16:
                    return "int16";
                case FoxDataType.FoxUInt16:
                    return "uint16";
                case FoxDataType.FoxInt32:
                    return "int32";
                case FoxDataType.FoxUInt32:
                    return "uint32";
                case FoxDataType.FoxInt64:
                    return "int64";
                case FoxDataType.FoxUInt64:
                    return "uint64";
                case FoxDataType.FoxFloat:
                    return "float";
                case FoxDataType.FoxDouble:
                    return "double";
                case FoxDataType.FoxBool:
                    return "bool";
                case FoxDataType.FoxString:
                    return "String";
                case FoxDataType.FoxPath:
                    return "Path";
                case FoxDataType.FoxEntityPtr:
                    return "EntityPtr";
                case FoxDataType.FoxVector3:
                    return "Vector3";
                case FoxDataType.FoxVector4:
                    return "Vector4";
                case FoxDataType.FoxQuat:
                    return "Quat";
                case FoxDataType.FoxMatrix3:
                    return "Matrix3";
                case FoxDataType.FoxMatrix4:
                    return "Matrix4";
                case FoxDataType.FoxColor:
                    return "Color";
                case FoxDataType.FoxFilePtr:
                    return "FilePtr";
                case FoxDataType.FoxEntityHandle:
                    return "EntityHandle";
                case FoxDataType.FoxEntityLink:
                    return "EntityLink";
                case FoxDataType.FoxPropertyInfo:
                    return "PropertyInfo";
                case FoxDataType.FoxWideVector3:
                    return "WideVector3";
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        internal static string ToStringRoundtrip(this float value)
        {
            if (value.Equals(0.0f) && BitConverter.GetBytes(value)[BitConverter.IsLittleEndian ? 3 : 0] == 0x80)
            {
                return "-0";
            }
            return value.ToString("r", CultureInfo.InvariantCulture);
        }

        internal static float ParseFloatRoundtrip(string text)
        {
            if (text == "-0")
            {
                return -0f;
            }
            return float.Parse(text, CultureInfo.InvariantCulture);
        }
    }
}
