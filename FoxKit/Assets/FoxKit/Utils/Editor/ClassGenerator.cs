﻿namespace FoxKit.Utils.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    using FoxKit.Modules.DataSet;
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.Lua;
    using FoxKit.Utils.Structs;

    using FoxLib;

    using OdinSerializer;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    using File = System.IO.File;

    public static class ClassGenerator
    {
        private const string GeneratedCodeWarning =
@"//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was automatically generated.
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------";

        private const string RootNamespace = "FoxKit.Modules.DataSet.Fox";

        private static readonly string[] usingNamespaces =
            {
                "System", "System.Collections.Generic", "FoxKit.Modules.DataSet.Fox.FoxCore", "FoxKit.Modules.Lua",
                "FoxLib", "static KopiLua.Lua", "OdinSerializer", "UnityEngine", "DataSetFile2 = DataSetFile2", "TppGameKit = FoxKit.Modules.DataSet.Fox.TppGameKit"
            };

        public static void GenerateClass(ClassDefinition definition, IDictionary<Core.PropertyInfoType, Type> typeMappings, Func<string, string> getNamespace, string outputDirectory)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(GeneratedCodeWarning);
            AppendNamespace(stringBuilder, definition.Namespace);
            stringBuilder.AppendLine("{");

            string parentNamespace = null;
            if (!string.IsNullOrEmpty(definition.Parent))
            {
                parentNamespace = getNamespace(definition.Parent);
            }

            AppendUsingStatements(stringBuilder, parentNamespace);
            AppendLineWithIndent(stringBuilder, string.Empty, 1);
            AppendClassAttributes(stringBuilder);
            AppendClassDeclaration(stringBuilder, definition.Name, definition.Parent);
            AppendLineWithIndent(stringBuilder, "{", 1);

            Func<string, string, string> parsePropertyType = delegate (string propertyTypeString, string ptrTypeString)
            {
                var propertyType = ParsePropertyType(propertyTypeString);
                if (propertyType == Core.PropertyInfoType.EntityPtr && !string.IsNullOrEmpty(ptrTypeString))
                {
                    var @namespace = getNamespace(ptrTypeString);
                    if (string.IsNullOrEmpty(@namespace))
                    {
                        return ptrTypeString;
                    }

                    return $"{getNamespace(ptrTypeString)}.{ptrTypeString}";
                }
                else
                {
                    return typeMappings[propertyType].ToString();
                }
            };

            for (var i = 0; i < definition.Properties.Count; i++)
            {
                var property = definition.Properties[i];
                AppendFieldDeclaration(stringBuilder, property, parsePropertyType, getNamespace);

                //if (!(i == definition.Properties.Count - 1 && definition.Functions.Count == 0))
                {
                    AppendLineWithIndent(stringBuilder, string.Empty, 2);
                }
            }

            // Add metadata properties.
            if (!string.IsNullOrEmpty(definition.Parent))
            {
                AppendLineWithIndent(stringBuilder, $"public override short ClassId => {definition.Id};", 2);
                AppendLineWithIndent(stringBuilder, string.Empty, 2);
                AppendLineWithIndent(stringBuilder, $"public override ushort Version => {definition.Version};", 2);
                AppendLineWithIndent(stringBuilder, string.Empty, 2);
                AppendLineWithIndent(stringBuilder, $"public override string Category => \"{definition.Category}\";", 2);
            }

            // Add methods.
            foreach (var function in definition.Functions)
            {
                // Exclude constructors.
                if (function.Name == definition.Name)
                {
                    continue;
                }

                AppendLineWithIndent(stringBuilder, string.Empty, 2);
                AppendFunctionDeclaration(stringBuilder, function);
            }

            AppendLineWithIndent(stringBuilder, "}", 1);
            stringBuilder.AppendLine("}");

            Directory.CreateDirectory($"{outputDirectory}/{definition.Namespace}");
            var outputFile = $"{outputDirectory}/{definition.Namespace}/{definition.Name}.Generated.cs";

            File.WriteAllText(outputFile, stringBuilder.ToString());
        }

        private static void AppendFunctionDeclaration(StringBuilder stringBuilder, FunctionDefinition function)
        {
            if (function.Type == "c")
            {
                AppendLineWithIndent(stringBuilder, "[ExposeMethodToLua(MethodStaticity.Static)]", 2);
                AppendLineWithIndent(stringBuilder, $"static partial void {function.Name}(lua_State lua);", 2);
            }
            else
            {
                AppendLineWithIndent(stringBuilder, "[ExposeMethodToLua(MethodStaticity.Instance)]", 2);
                AppendLineWithIndent(stringBuilder, $"partial void {function.Name}(lua_State lua);", 2);
            }
        }

        private static void AppendFieldDeclaration(StringBuilder stringBuilder, PropertyDefinition property, Func<string, string, string> parsePropertyType, Func<string, string> getNamespace)
        {
            var containerType = ParseContainerType(property.Container);
            AppendFieldAttributes(stringBuilder, property, containerType, getNamespace);

            var propertyName = property.Name;

            // FIXME dumb hack
            if (propertyName == "string" || propertyName == "params" || propertyName == "object")
            {
                propertyName = $"@{propertyName}";
            }

            var propertyType = parsePropertyType(property.Type, property.PtrType);
            if (property.Enum != "null" && property.Enum != null)
            {
                propertyType = property.Enum;
            }

            var typeDeclaration = MakeTypeDeclaration(
                propertyType,
                containerType,
                property.ArraySize);

            AppendWithIndent(stringBuilder, $"private {typeDeclaration} {propertyName}", 2);

            if (containerType == Core.ContainerType.StaticArray && property.ArraySize == 1)
            {
                if (property.Type == "String")
                {
                    stringBuilder.Append(" = string.Empty");
                }
                stringBuilder.AppendLine(";");
            }
            else
            {
                stringBuilder.AppendLine($" = new {typeDeclaration}();");
            }
        }

        private static string MakeTypeDeclaration(string innerType, Core.ContainerType containerType, uint arraySize)
        {
            if (containerType == Core.ContainerType.StaticArray && arraySize == 1)
            {
                return innerType;
            }

            if (containerType == Core.ContainerType.StringMap)
            {
                return $"Dictionary<string, {innerType}>";
            }

            return $"List<{innerType}>";
        }

        private static void AppendFieldAttributes(StringBuilder stringBuilder, PropertyDefinition property, Core.ContainerType containerType, Func<string, string> getNamespace)
        {
            var propertyInfoStringBuilder = new StringBuilder();
            propertyInfoStringBuilder.Append($"{nameof(PropertyInfoAttribute)}(");
            propertyInfoStringBuilder.Append($"Core.PropertyInfoType.{ParsePropertyType(property.Type)}");
            propertyInfoStringBuilder.Append(", ");
            propertyInfoStringBuilder.Append(property.Offset);
            propertyInfoStringBuilder.Append(", ");
            propertyInfoStringBuilder.Append(property.ArraySize);
            propertyInfoStringBuilder.Append(", ");
            propertyInfoStringBuilder.Append($"Core.ContainerType.{containerType}");
            propertyInfoStringBuilder.Append(", ");
            propertyInfoStringBuilder.Append($"PropertyExport.{ParseReadableFlag(property.ExportFlag)}");
            propertyInfoStringBuilder.Append(", ");
            propertyInfoStringBuilder.Append($"PropertyExport.{ParseWritableFlag(property.ExportFlag)}");
            propertyInfoStringBuilder.Append(", ");

            var ptrType = ParsePtrType(property.PtrType);
            var ptrTypeString = "null";
            if (ptrType != "null")
            {
                var ptrTypeNamespace = getNamespace(ptrType);
                ptrTypeString = ptrType;
                if (!string.IsNullOrEmpty(ptrTypeNamespace))
                {
                    ptrTypeString = $"typeof({ptrTypeNamespace}.{ptrTypeString})";
                }
                else
                {
                    ptrTypeString = $"typeof({ptrTypeString})";
                }
            }

            propertyInfoStringBuilder.Append(ptrTypeString);
            propertyInfoStringBuilder.Append(", ");
            propertyInfoStringBuilder.Append(string.IsNullOrEmpty(property.Enum) ? "null" : $"typeof({property.Enum})");
            propertyInfoStringBuilder.Append(")");

            AppendLineWithIndent(stringBuilder, $"[{nameof(OdinSerializeAttribute)}, {nameof(NonSerializedAttribute)}, {propertyInfoStringBuilder}]", 2);
        }

        public static IDictionary<Core.PropertyInfoType, Type> CreateFoxToUnityTypeMapping()
        {
            var result = new Dictionary<Core.PropertyInfoType, Type>();
            result.Add(Core.PropertyInfoType.Bool, typeof(bool));
            result.Add(Core.PropertyInfoType.Vector3, typeof(UnityEngine.Vector3));
            result.Add(Core.PropertyInfoType.Vector4, typeof(UnityEngine.Vector4));
            result.Add(Core.PropertyInfoType.Quat, typeof(UnityEngine.Quaternion));
            result.Add(Core.PropertyInfoType.Color, typeof(UnityEngine.Color));
            result.Add(Core.PropertyInfoType.Matrix3, typeof(Matrix3x3));
            result.Add(Core.PropertyInfoType.Matrix4, typeof(UnityEngine.Matrix4x4));
            result.Add(Core.PropertyInfoType.UInt32, typeof(uint));
            result.Add(Core.PropertyInfoType.Double, typeof(double));
            result.Add(Core.PropertyInfoType.EntityHandle, typeof(Entity));
            result.Add(Core.PropertyInfoType.EntityPtr, typeof(Entity));
            result.Add(Core.PropertyInfoType.EntityLink, typeof(EntityLink));
            result.Add(Core.PropertyInfoType.FilePtr, typeof(UnityEngine.Object));
            result.Add(Core.PropertyInfoType.Path, typeof(UnityEngine.Object));
            result.Add(Core.PropertyInfoType.String, typeof(string));
            result.Add(Core.PropertyInfoType.Float, typeof(float));
            result.Add(Core.PropertyInfoType.Int16, typeof(short));
            result.Add(Core.PropertyInfoType.UInt16, typeof(ushort));
            result.Add(Core.PropertyInfoType.Int32, typeof(int));
            result.Add(Core.PropertyInfoType.Int8, typeof(sbyte));
            result.Add(Core.PropertyInfoType.UInt8, typeof(byte));
            result.Add(Core.PropertyInfoType.Int64, typeof(long));
            result.Add(Core.PropertyInfoType.UInt64, typeof(ulong));
            result.Add(Core.PropertyInfoType.PropertyInfo, null);
            result.Add(Core.PropertyInfoType.WideVector3, typeof(object));
            return result;
        }

        private static FoxLib.Core.PropertyInfoType ParsePropertyType(string type)
        {
            switch (type)
            {
                case "int8":
                    return Core.PropertyInfoType.Int8;
                case "uint8":
                    return Core.PropertyInfoType.UInt8;
                case "int16":
                    return Core.PropertyInfoType.Int16;
                case "uint16":
                    return Core.PropertyInfoType.UInt16;
                case "int32":
                    return Core.PropertyInfoType.Int32;
                case "uint32":
                    return Core.PropertyInfoType.UInt32;
                case "int64":
                    return Core.PropertyInfoType.Int64;
                case "uint64":
                    return Core.PropertyInfoType.UInt64;
                case "float":
                    return Core.PropertyInfoType.Float;
                case "double":
                    return Core.PropertyInfoType.Double;
                case "bool":
                    return Core.PropertyInfoType.Bool;
                case "Color":
                    return Core.PropertyInfoType.Color;
                case "Vector3":
                    return Core.PropertyInfoType.Vector3;
                case "Vector4":
                    return Core.PropertyInfoType.Vector4;
                case "Quat":
                    return Core.PropertyInfoType.Quat;
                case "Matrix3":
                    return Core.PropertyInfoType.Matrix3;
                case "Matrix4":
                    return Core.PropertyInfoType.Matrix4;
                case "Path":
                    return Core.PropertyInfoType.Path;
                case "String":
                    return Core.PropertyInfoType.String;
                case "FilePtr":
                    return Core.PropertyInfoType.FilePtr;
                case "EntityPtr":
                    return Core.PropertyInfoType.EntityPtr;
                case "EntityHandle":
                    return Core.PropertyInfoType.EntityHandle;
                case "EntityLink":
                    return Core.PropertyInfoType.EntityLink;
                case "WideVector3":
                    return Core.PropertyInfoType.WideVector3;
                case "PropertyInfo":
                    return Core.PropertyInfoType.PropertyInfo;
                default:
                    Assert.IsFalse(true, $"Unrecognized type: {type}");
                    return Core.PropertyInfoType.PropertyInfo;
            }
        }

        private static Core.ContainerType ParseContainerType(string container)
        {
            Core.ContainerType result;
            if (Enum.TryParse(container, true, out result))
            {
                return result;
            }

            Debug.LogError($"Unrecognized container type {container}.");
            return Core.ContainerType.StaticArray;
        }

        private static PropertyExport ParseReadableFlag(string exportFlag)
        {
            switch (exportFlag[0])
            {
                case 'R':
                    return PropertyExport.EditorAndGame;
                case 'r':
                    return PropertyExport.EditorOnly;
            }
            return PropertyExport.Never;
        }

        private static PropertyExport ParseWritableFlag(string exportFlag)
        {
            switch (exportFlag[1])
            {
                case 'W':
                    return PropertyExport.EditorAndGame;
                case 'w':
                    return PropertyExport.EditorOnly;
            }
            return PropertyExport.Never;
        }

        private static string ParsePtrType(string ptrType)
        {
            if (string.IsNullOrEmpty(ptrType))
            {
                return "null";
            }

            return ptrType; //$"typeof({ptrType})";
        }

        private static void AppendClassDeclaration(
            StringBuilder stringBuilder,
            string className,
            string parentClassName)
        {
            if (string.IsNullOrEmpty(parentClassName))
            {
                AppendLineWithIndent(stringBuilder, $"public partial class {className}", 1);
                return;
            }

            AppendLineWithIndent(stringBuilder, $"public partial class {className} : {parentClassName}", 1);
        }

        private static void AppendClassAttributes(StringBuilder stringBuilder)
        {
            AppendLineWithIndent(stringBuilder, $"[{nameof(SerializableAttribute)}, {nameof(ExposeClassToLuaAttribute)}]", 1);
        }

        private static void AppendUsingStatements(StringBuilder stringBuilder, string parentNamespace)
        {
            foreach (var @namespace in usingNamespaces)
            {
                AppendUsingStatement(stringBuilder, @namespace);
            }

            if (string.IsNullOrEmpty(parentNamespace))
            {
                return;
            }

            AppendUsingStatement(stringBuilder, parentNamespace);
        }

        private static void AppendUsingStatement(StringBuilder stringBuilder, string @namespace)
        {
            AppendLineWithIndent(stringBuilder, $"using {@namespace};", 1);
        }

        private static void AppendWithIndent(StringBuilder stringBuilder, string text, int indentLevel)
        {
            Indent(stringBuilder, indentLevel);
            stringBuilder.Append(text);
        }

        private static void AppendLineWithIndent(StringBuilder stringBuilder, string text, int indentLevel)
        {
            Indent(stringBuilder, indentLevel);
            stringBuilder.AppendLine(text);
        }

        private static void AppendNamespace(StringBuilder stringBuilder, string @namespace)
        {
            stringBuilder.AppendLine($"namespace {GetFullNamespace(@namespace)}");
        }

        public static string GetFullNamespace(string @namespace)
        {
            return string.IsNullOrEmpty(@namespace) ? $"{RootNamespace}" : $"{RootNamespace}.{@namespace}";
        }

        private static void Indent(StringBuilder stringBuilder, int indentLevel)
        {
            for (var i = 0; i < indentLevel; i++)
            {
                stringBuilder.Append("    ");
            }
        }

        [DataContract]
        public class ClassDefinition
        {
            public const short UnknownId = short.MaxValue;

            public const ushort UnknownVersion = 0;

            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "parent")]
            public string Parent { get; set; }

            [DataMember(Name = "namespace")]
            public string Namespace { get; set; }

            [DataMember(Name = "category")]
            public string Category { get; set; }

            [DataMember(Name = "id")]
            public short Id { get; set; } = UnknownId;

            [DataMember(Name = "version")]
            public ushort Version { get; set; } = UnknownVersion;

            [DataMember(Name = "properties")]
            public List<PropertyDefinition> Properties { get; set; }

            [DataMember(Name = "functions")]
            public List<FunctionDefinition> Functions { get; set; }
        }

        [DataContract]
        public class PropertyDefinition
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "type")]
            public string Type { get; set; }

            [DataMember(Name = "container")]
            public string Container { get; set; }

            [DataMember(Name = "arraySize")]
            public uint ArraySize { get; set; }

            [DataMember(Name = "exportFlag")]
            public string ExportFlag { get; set; }

            [DataMember(Name = "ptrType")]
            public string PtrType { get; set; }

            [DataMember(Name = "enum")]
            public string Enum { get; set; }

            [DataMember(Name = "offset")]
            public uint Offset { get; set; }

            [DataMember(Name = "size")]
            public uint Size { get; set; }
        }

        [DataContract]
        public class FunctionDefinition
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "type")]
            public string Type { get; set; }
        }
    }

    public class ClassGeneratorWindow : EditorWindow
    {
        private TextAsset classDefinitions;

        private TextAsset classNamespaces;

        private TextAsset classVersions;

        private TextAsset classIds;

        private TextAsset classCategories;

        [MenuItem("Window/FoxKit/Developer/Class Generator")]
        private static void Init()
        {
            var window = (ClassGeneratorWindow)EditorWindow.GetWindow(typeof(ClassGeneratorWindow));
            window.titleContent = new GUIContent("Class Generator");
            window.Show();
        }

        void OnGUI()
        {
            this.classDefinitions = EditorGUILayout.ObjectField("Class Definitions", this.classDefinitions, typeof(TextAsset), false) as TextAsset;
            /*this.classVersions = EditorGUILayout.ObjectField("Class Versions", this.classVersions, typeof(TextAsset), false) as TextAsset;
            this.classIds = EditorGUILayout.ObjectField("Class IDs", this.classIds, typeof(TextAsset), false) as TextAsset;
            this.classCategories = EditorGUILayout.ObjectField("Class Categories", this.classCategories, typeof(TextAsset), false) as TextAsset;

            if (GUILayout.Button("Parse Metadata"))
            {
                var outputPath = EditorUtility.SaveFilePanelInProject("Select output file", null, "json", "Select output file");
                if (outputPath.Length == 0)
                {
                    return;
                }

                var baseDefs = ReadClassDefinitions(this.classDefinitions);
                var parsed = ParseAndAssigMetadata(
                    baseDefs,
                    this.classIds,
                    this.classVersions,
                    this.classCategories);

                var settings = new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true };
                using (var stream = new FileStream(outputPath, FileMode.Create))
                {
                    using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "  "))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<ClassGenerator.ClassDefinition>), settings);
                        serializer.WriteObject(writer, parsed);
                        writer.Flush();
                    }
                }

                return;
            }*/

            if (!GUILayout.Button("Generate Classes"))
            {
                return;
            }

            var path = EditorUtility.OpenFolderPanel("Select output folder", null, null);
            if (path.Length == 0)
            {
                return;
            }

            var definitions = ReadClassDefinitions(this.classDefinitions).ToDictionary(entry => entry.Name, entry => entry);
            var mappings = ClassGenerator.CreateFoxToUnityTypeMapping();
            Func<string, string> getNamespace = className => definitions[className].Namespace;

            foreach (var definition in definitions.Values)
            {
                ClassGenerator.GenerateClass(definition, mappings, getNamespace, path);
            }

            AssetDatabase.Refresh();
        }

        private static List<ClassGenerator.ClassDefinition> ParseAndAssigMetadata(List<ClassGenerator.ClassDefinition> definitions, TextAsset classIds, TextAsset classVersions, TextAsset classCategories)
        {
            var ids = ParseClassIds(classIds);
            var versions = ParseClassVersions(classVersions);
            var categories = ParseClassCategories(classCategories);

            foreach (var definition in definitions)
            {
                if (ids.ContainsKey(definition.Name))
                {
                    definition.Id = ids[definition.Name];
                }

                if (versions.ContainsKey(definition.Name))
                {
                    definition.Version = versions[definition.Name];
                }

                if (categories.ContainsKey(definition.Name))
                {
                    definition.Category = categories[definition.Name];
                }
            }

            return definitions;
        }

        private static List<ClassGenerator.ClassDefinition> ParseAndAssignNamespaces(TextAsset classDefinitions, TextAsset classNamespaces)
        {
            var namespaces = ParseClassNamespaces(classNamespaces);
            var definitions = ReadClassDefinitions(classDefinitions);

            foreach (var definition in definitions)
            {
                definition.Namespace = namespaces[definition.Name];
            }

            return definitions;
        }

        private static List<ClassGenerator.ClassDefinition> ReadClassDefinitions(TextAsset classDefinitions)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<ClassGenerator.ClassDefinition>));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(classDefinitions.text)))
            {
                return (List<ClassGenerator.ClassDefinition>)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Parses a class namespace list into a dictionary mapping class name to its namespace.
        /// </summary>
        /// <param name="classNamespaces"></param>
        /// <returns></returns>
        private static IDictionary<string, string> ParseClassNamespaces(TextAsset classNamespaces)
        {
            var lines = new List<string>(classNamespaces.text.Split('\n'));
            char[] delimiterChars = { ' ', '(', ')', '\'', ',' };

            var result = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var splitLine = line.Split(delimiterChars);
                Assert.IsTrue(splitLine.Length > 5);

                var className = splitLine[2];
                var namespaceName = splitLine[6];

                if (string.IsNullOrWhiteSpace(namespaceName))
                {
                    namespaceName = null;
                }

                result.Add(className, namespaceName);
            }

            return result;
        }

        private static IDictionary<string, short> ParseClassIds(TextAsset classIds)
        {
            var lines = new List<string>(classIds.text.Split('\n'));
            char[] delimiterChars = { ' ' };

            var result = new Dictionary<string, short>();
            foreach (var line in lines)
            {
                var splitLine = line.Split(delimiterChars);
                Assert.IsTrue(splitLine.Length == 2);

                var className = splitLine[0];
                var id = splitLine[1];

                result.Add(className, short.Parse(id));
            }

            return result;
        }

        private static IDictionary<string, ushort> ParseClassVersions(TextAsset classIds)
        {
            var lines = new List<string>(classIds.text.Split('\n'));
            char[] delimiterChars = { ' ', '\r' };

            var result = new Dictionary<string, ushort>();
            foreach (var line in lines)
            {
                var splitLine = line.Split(delimiterChars);

                var className = splitLine[0];
                var id = splitLine[1];

                if (id == "None")
                {
                    continue;
                }

                result.Add(className, ushort.Parse(id));
            }

            return result;
        }

        private static IDictionary<string, string> ParseClassCategories(TextAsset categories)
        {
            var lines = new List<string>(categories.text.Split('\n'));
            char[] delimiterChars = { ' ', '\r' };

            var result = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var splitLine = line.Split(delimiterChars);
                var className = splitLine[0];
                var category = splitLine[1];

                if (string.IsNullOrWhiteSpace(category))
                {
                    category = null;
                }

                result.Add(className, category);
            }

            return result;
        }
    }
}