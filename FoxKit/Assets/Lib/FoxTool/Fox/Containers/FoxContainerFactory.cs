using System;
using System.IO;
using FoxTool.Fox.Types;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Fox.Containers
{
    internal static class FoxContainerFactory
    {
        public static IFoxContainer ReadFoxContainer(Stream input, FoxDataType dataType, FoxContainerType containerType,
            short valueCount)
        {
            var container = CreateTypedContainer(dataType, containerType);
            container.Read(input, valueCount);
            return container;
        }

        public static IFoxContainer CreateTypedContainer(FoxDataType dataType, FoxContainerType containerType)
        {
            IFoxContainer container;
            switch (dataType)
            {
                case FoxDataType.FoxInt8:
                    container = CreateContainer<FoxInt8>(containerType);
                    break;
                case FoxDataType.FoxUInt8:
                    container = CreateContainer<FoxUInt8>(containerType);
                    break;
                case FoxDataType.FoxInt16:
                    container = CreateContainer<FoxInt16>(containerType);
                    break;
                case FoxDataType.FoxUInt16:
                    container = CreateContainer<FoxUInt16>(containerType);
                    break;
                case FoxDataType.FoxInt32:
                    container = CreateContainer<FoxInt32>(containerType);
                    break;
                case FoxDataType.FoxUInt32:
                    container = CreateContainer<FoxUInt32>(containerType);
                    break;
                case FoxDataType.FoxInt64:
                    container = CreateContainer<FoxInt64>(containerType);
                    break;
                case FoxDataType.FoxUInt64:
                    container = CreateContainer<FoxUInt64>(containerType);
                    break;
                case FoxDataType.FoxFloat:
                    container = CreateContainer<FoxFloat>(containerType);
                    break;
                case FoxDataType.FoxDouble:
                    container = CreateContainer<FoxDouble>(containerType);
                    break;
                case FoxDataType.FoxBool:
                    container = CreateContainer<FoxBool>(containerType);
                    break;
                case FoxDataType.FoxString:
                    container = CreateContainer<FoxString>(containerType);
                    break;
                case FoxDataType.FoxPath:
                    container = CreateContainer<FoxPath>(containerType);
                    break;
                case FoxDataType.FoxEntityPtr:
                    container = CreateContainer<FoxEntityPtr>(containerType);
                    break;
                case FoxDataType.FoxVector3:
                    container = CreateContainer<FoxVector3>(containerType);
                    break;
                case FoxDataType.FoxVector4:
                    container = CreateContainer<FoxVector4>(containerType);
                    break;
                case FoxDataType.FoxQuat:
                    container = CreateContainer<FoxQuat>(containerType);
                    break;
                case FoxDataType.FoxMatrix3:
                    container = CreateContainer<FoxMatrix3>(containerType);
                    break;
                case FoxDataType.FoxMatrix4:
                    container = CreateContainer<FoxMatrix4>(containerType);
                    break;
                case FoxDataType.FoxColor:
                    container = CreateContainer<FoxColor>(containerType);
                    break;
                case FoxDataType.FoxFilePtr:
                    container = CreateContainer<FoxFilePtr>(containerType);
                    break;
                case FoxDataType.FoxEntityHandle:
                    container = CreateContainer<FoxEntityHandle>(containerType);
                    break;
                case FoxDataType.FoxEntityLink:
                    container = CreateContainer<FoxEntityLink>(containerType);
                    break;
                case FoxDataType.FoxPropertyInfo:
                    container = CreateContainer<FoxPropertyInfo>(containerType);
                    break;
                case FoxDataType.FoxWideVector3:
                    container = CreateContainer<FoxWideVector3>(containerType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dataType");
            }
            return container;
        }

        private static IFoxContainer CreateContainer<T>(FoxContainerType containerType) where T : IFoxValue, new()
        {
            switch (containerType)
            {
                case FoxContainerType.StaticArray:
                    return new FoxStaticArray<T>();
                case FoxContainerType.DynamicArray:
                    return new FoxDynamicArray<T>();
                case FoxContainerType.StringMap:
                    return new FoxStringMap<T>();
                case FoxContainerType.List:
                    return new FoxList<T>();
                default:
                    throw new ArgumentOutOfRangeException("containerType");
            }
        }
    }
}
