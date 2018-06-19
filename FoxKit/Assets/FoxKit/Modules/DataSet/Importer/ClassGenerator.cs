namespace FoxKit.Modules.DataSet.Importer
{
    using System;
    using System.IO;
    using System.Text;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    public static class ClassGenerator
    {
        private static readonly string OutputDirectory = Application.dataPath + @"/FoxKit/Modules/DataSet/Generated/";

        public static void GenerateClassFromEntity(Core.Entity entity)
        {
            var sourceCode = GenerateClassSourceCode(entity);
            File.WriteAllText(OutputDirectory + $"{entity.ClassName}.cs", sourceCode);
            
            // TODO Don't do this each time
            AssetDatabase.Refresh();
        }

        private static string GenerateClassSourceCode(Core.Entity entity)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(MakeNamespaceStatement());

            // Open namespace block.
            stringBuilder.AppendLine("{");
            
            AddUsingStatements(stringBuilder);

            stringBuilder.AppendLine("    /// <inheritdoc />");
            stringBuilder.AppendLine("    [Serializable]");

            // TODO Determine superclass
            stringBuilder.AppendLine(MakeClassStatement(entity.ClassName, "Data"));

            // Open class block.
            stringBuilder.AppendLine("    {");

            AddStaticPropertyFields(stringBuilder, entity.StaticProperties, entity.ClassName);

            AddProperties(entity.ClassId, entity.Version, stringBuilder);

            // TODO MakeWritableStaticProperties
            // TODO ReadProperty
            // TODO OnAssetsImported (if there are files referenced)

            // Close class block.
            stringBuilder.AppendLine("    }");

            // Close namespace block.
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

        private static void AddProperties(short classId, ushort version, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("        /// <inheritdoc />");
            stringBuilder.AppendLine(MakeClassIdDeclaration(classId));
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("        /// <inheritdoc />");
            stringBuilder.AppendLine(MakeVersionDeclaration(version));
        }

        private static void AddStaticPropertyFields(StringBuilder stringBuilder, Core.PropertyInfo[] properties, string className)
        {
            foreach (var property in properties)
            {
                AddStaticPropertyField(stringBuilder, property, className);
                stringBuilder.AppendLine(string.Empty);
            }
        }

        private static void AddStaticPropertyField(StringBuilder stringBuilder, Core.PropertyInfo property, string className)
        {
            // TODO Initialize string to string.Empty
            // TODO Handle entity ptrs
            // TODO Containers
            stringBuilder.AppendLine($"        [SerializeField, Modules.DataSet.Property(\"{className}\")]");
            stringBuilder.AppendLine($"        private {GetTypeString(property.Type)} {property.Name};");
        }

        private static string GetTypeString(Core.PropertyInfoType type)
        {
            switch (type)
            {
                case Core.PropertyInfoType.Int8:
                    return "sbyte";
                case Core.PropertyInfoType.UInt8:
                    return "byte";
                case Core.PropertyInfoType.Int16:
                    return "short";
                case Core.PropertyInfoType.UInt16:
                    return "ushort";
                case Core.PropertyInfoType.Int32:
                    return "int";
                case Core.PropertyInfoType.UInt32:
                    return "uint";
                case Core.PropertyInfoType.Int64:
                    return "long";
                case Core.PropertyInfoType.UInt64:
                    return "ulong";
                case Core.PropertyInfoType.Float:
                    return "float";
                case Core.PropertyInfoType.Double:
                    return "double";
                case Core.PropertyInfoType.Bool:
                    return "bool";
                case Core.PropertyInfoType.String:
                    return "string";
                case Core.PropertyInfoType.Path:
                    return "string";
                case Core.PropertyInfoType.EntityPtr:
                    return "object";    // TODO
                case Core.PropertyInfoType.Vector3:
                    return "Vector3";
                case Core.PropertyInfoType.Vector4:
                    return "Vector4";
                case Core.PropertyInfoType.Quat:
                    return "Quaternion";
                case Core.PropertyInfoType.Matrix3:
                    return "Matrix3x3";
                case Core.PropertyInfoType.Matrix4:
                    return "Matrix4x4";
                case Core.PropertyInfoType.Color:
                    return "Color";
                case Core.PropertyInfoType.FilePtr:
                    return "UnityEngine.Object";
                case Core.PropertyInfoType.EntityHandle:
                    return "object";    // TODO
                case Core.PropertyInfoType.EntityLink:
                    return "EntityLink";
                case Core.PropertyInfoType.PropertyInfo:
                    Assert.IsTrue(false, "Unsupported property type: PropertyInfo.");
                    break;
                case Core.PropertyInfoType.WideVector3:
                    Assert.IsTrue(false, "Unsupported property type: WideVector3.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return "ERROR";
        }

        private static void AddUsingStatements(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine(MakeUsingStatement("System"));
            stringBuilder.AppendLine(MakeUsingStatement("System.Collections.Generic"));
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(MakeUsingStatement("FoxKit.Modules.DataSet.Exporter"));
            stringBuilder.AppendLine(MakeUsingStatement("FoxKit.Modules.DataSet.FoxCore"));
            stringBuilder.AppendLine(MakeUsingStatement("FoxKit.Utils"));
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(MakeUsingStatement("FoxLib"));
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(MakeUsingStatement("UnityEditor"));
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(MakeUsingStatement("UnityEngine"));
            stringBuilder.AppendLine(string.Empty);
        }

        private static string MakeNamespaceStatement()
        {
            // TODO: Generate correct namespace
            return "namespace FoxKit.Modules.DataSet";
        }

        private static string MakeUsingStatement(string @namespace)
        {
            return $"    using {@namespace};";
        }

        private static string MakeClassStatement(string className, string parentClassName)
        {
            return $"    public class {className} : {parentClassName}";
        }

        private static string MakeClassIdDeclaration(short classId)
        {
            return $"        public override short ClassId => {classId};";
        }

        private static string MakeVersionDeclaration(ushort version)
        {
            return $"        public override ushort Version => {version};";
        }
    }
}