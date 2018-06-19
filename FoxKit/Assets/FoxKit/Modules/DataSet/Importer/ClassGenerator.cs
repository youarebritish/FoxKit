namespace FoxKit.Modules.DataSet.Importer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.PartsBuilder;

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

            var parentClass =
                DetermineBaseClass((from property in entity.StaticProperties
                                    select property.Name).ToList());
            stringBuilder.AppendLine(MakeClassStatement(entity.ClassName, parentClass.Name));

            // Open class block.
            stringBuilder.AppendLine("    {");

            var staticProperties = GetPrunedStaticPropertyFields(entity.StaticProperties, parentClass);
            AddStaticPropertyFields(stringBuilder, staticProperties.ToArray(), entity.ClassName);

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

        private static IEnumerable<Core.PropertyInfo> GetPrunedStaticPropertyFields(
            Core.PropertyInfo[] staticProperties,
            Type parentClass)
        {
            var propertiesToPrune = new List<string>();
            if (parentClass == typeof(DataElement))
            {
                propertiesToPrune.AddRange(DataElementPropertyNames);
            }
            else if (parentClass == typeof(Data))
            {
                propertiesToPrune.AddRange(DataPropertyNames);
            }
            else if (parentClass == typeof(TransformData))
            {
                propertiesToPrune.AddRange(DataPropertyNames);
                propertiesToPrune.AddRange(TransformDataPropertyNames);
            }
            else if (parentClass == typeof(PartDescription))
            {
                propertiesToPrune.AddRange(DataPropertyNames);
                propertiesToPrune.AddRange(PartDescriptionPropertyNames);
            }

            return from property in staticProperties where !propertiesToPrune.Contains(property.Name) select property;
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
            stringBuilder.AppendLine($"        [SerializeField, Modules.DataSet.Property(\"{className}\")]");
            stringBuilder.AppendLine($"        private {GetPropertyFieldTypeString(property.ContainerType, property.Type, property.Container)} {property.Name};");
        }

        private static string GetPropertyFieldTypeString(
            Core.ContainerType containerType,
            Core.PropertyInfoType propertyType,
            Core.IContainer container)
        {
            var innerTypeString = GetTypeString(propertyType);

            if (containerType == Core.ContainerType.StaticArray && container.ArraySize == 1)
            {
                return innerTypeString;
            }

            if (containerType != Core.ContainerType.StringMap)
            {
                return $"List<{innerTypeString}>";
            }

            // TODO this is terrible and wrong
            return "ObjectStringMap";
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
                    return "Entity";    // TODO
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
                    return "Entity";    // TODO
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

        private static readonly List<string> DataElementPropertyNames = new List<string>{"owner"};
        private static readonly List<string> DataPropertyNames = new List<string> { "name", "dataSet" };
        private static readonly List<string> TransformDataPropertyNames = new List<string> { "parent", "transform", "shearTransform", "pivotTransform", "children", "flags" };
        private static readonly List<string> PartDescriptionPropertyNames = new List<string> { "depends", "partName", "buildType" };

        private static Type DetermineBaseClass(ICollection<string> staticPropertyNames)
        {
            // DataElement
            if (DataElementPropertyNames.All(staticPropertyNames.Contains))
            {
                return typeof(DataElement);
            }

            // Data
            if (!DataPropertyNames.All(staticPropertyNames.Contains))
            {
                return typeof(Entity);
            }

            if (TransformDataPropertyNames.All(staticPropertyNames.Contains))
            {
                return typeof(TransformData);
            }

            return PartDescriptionPropertyNames.All(staticPropertyNames.Contains) ? typeof(PartDescription) : typeof(Data);
        }
    }
}