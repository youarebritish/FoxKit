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

        public static void GenerateClassFromEntity(Core.Entity entity, string filename)
        {
            var sourceCode = GenerateClassSourceCode(entity, filename);

            using (var writer = new StreamWriter(new FileStream(OutputDirectory + $"{entity.ClassName}.cs", FileMode.OpenOrCreate)))
            {
                writer.Write(sourceCode);
            }
            
            // TODO Don't do this each time
            AssetDatabase.Refresh();
        }

        private static string GenerateClassSourceCode(Core.Entity entity, string filename)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(MakeNamespaceStatement());

            // Open namespace block.
            stringBuilder.AppendLine("{");
            
            AddUsingStatements(stringBuilder);

            stringBuilder.AppendLine($"    // Automatically generated from file {filename}");
            stringBuilder.AppendLine("    /// <inheritdoc />");
            stringBuilder.AppendLine("    [Serializable]");
            
            var parentClass =
                DetermineBaseClass((from property in entity.StaticProperties
                                    select property.Name).ToList());
            stringBuilder.AppendLine(MakeClassStatement(entity.ClassName, parentClass.Name));

            // Open class block.
            stringBuilder.AppendLine("    {");

            var staticProperties = GetPrunedStaticPropertyFields(entity.StaticProperties, parentClass).ToArray();
            AddStaticPropertyFields(stringBuilder, staticProperties, entity.ClassName);

            AddProperties(entity.ClassId, entity.Version, stringBuilder);
            stringBuilder.AppendLine(string.Empty);

            AddOnAssetsImportedFunction(stringBuilder, staticProperties);

            AddMakeWritableStaticPropertiesFunction(stringBuilder, staticProperties);
            stringBuilder.AppendLine(string.Empty);

            AddReadPropertyFunction(stringBuilder, staticProperties);
            
            // Close class block.
            stringBuilder.AppendLine("    }");

            // Close namespace block.
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

        private static void AddReadPropertyFunction(StringBuilder stringBuilder, IEnumerable<Core.PropertyInfo> staticProperties)
        {
            stringBuilder.AppendLine("        /// <inheritdoc />");
            stringBuilder.AppendLine(
                "        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)");

            // Open function block.
            stringBuilder.AppendLine("        {");

            stringBuilder.AppendLine("            base.ReadProperty(propertyData, initFunctions);");
            stringBuilder.AppendLine(string.Empty);

            stringBuilder.AppendLine("            switch (propertyData.Name)");

            // Open switch statement block.
            stringBuilder.AppendLine("            {");

            foreach (var property in staticProperties)
            {
                var propertyName = "_" + property.Name;
                if (property.Type == Core.PropertyInfoType.FilePtr || property.Type == Core.PropertyInfoType.Path)
                {
                    propertyName = property.Name + "Path";
                }

                AddReadPropertyBlock(stringBuilder, property, propertyName);
            }

            // Close switch statement block.
            stringBuilder.AppendLine("            }");

            // Close function block.
            stringBuilder.AppendLine("        }");
        }

        private static void AddReadPropertyBlock(StringBuilder stringBuilder, Core.PropertyInfo property, string backingPropertyName)
        {
            stringBuilder.AppendLine($"                case \"{property.Name}\":");

            var rawTypeString = GetRawTypeString(property.Type);
            var convertedTypeString = GetConvertedTypeString(property.Type, false);

            // In the case of FilePtrs, because we need an extra property for the path, override the type string.
            if (property.Type == Core.PropertyInfoType.FilePtr)
            {
                convertedTypeString = "string";
            }

            var conversionFunctionString = GetFoxToUnityConversionFunctionString(property.Type);

            if (property.ContainerType == Core.ContainerType.StringMap)
            {
                stringBuilder.AppendLine($"                    var {property.Name}Dictionary = DataSetUtils.GetStringMap<{rawTypeString}>(propertyData);");
                stringBuilder.AppendLine($"                    var {property.Name}FinalValues = new OrderedDictionary_string_{convertedTypeString}();");
                stringBuilder.AppendLine("                    ");
                stringBuilder.AppendLine($"                    foreach(var entry in {property.Name}Dictionary)");
                stringBuilder.AppendLine("                    {");
                stringBuilder.AppendLine($"                        {property.Name}FinalValues.Add(entry.Key, {conversionFunctionString}(entry.Value));");
                stringBuilder.AppendLine("                    }");
                stringBuilder.AppendLine("                    ");
                stringBuilder.AppendLine($"                    this.{backingPropertyName} = {property.Name}FinalValues;");
            }
            else if (property.ContainerType == Core.ContainerType.StaticArray && property.Container.ArraySize == 1)
            {
                stringBuilder.AppendLine($"                    this.{backingPropertyName} = {conversionFunctionString}(DataSetUtils.GetStaticArrayPropertyValue<{rawTypeString}>(propertyData));");
            }
            else
            {
                var extractValuesFunctionString = "DataSetUtils.GetStaticArrayValues";
                if (property.ContainerType == Core.ContainerType.DynamicArray)
                {
                    extractValuesFunctionString = "DataSetUtils.GetDynamicArrayValues";
                }
                else if (property.ContainerType == Core.ContainerType.List)
                {
                    extractValuesFunctionString = "DataSetUtils.GetListValues";
                }

                stringBuilder.AppendLine($"                    this.{backingPropertyName} = (from rawValue in {extractValuesFunctionString}<{rawTypeString}>(propertyData) select {conversionFunctionString}(rawValue)).ToList();");
            }

            stringBuilder.AppendLine("                    break;");
        }

        private static void AddOnAssetsImportedFunction(StringBuilder stringBuilder, Core.PropertyInfo[] properties)
        {
            var filePtrProperties = (from property in properties
                                     where property.Type == Core.PropertyInfoType.FilePtr
                                    select property).ToList();

            if (filePtrProperties.Count == 0)
            {
                return;
            }

            stringBuilder.AppendLine("        /// <inheritdoc />");
            stringBuilder.AppendLine("        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            base.OnAssetsImported(tryGetAsset);");
            stringBuilder.AppendLine(string.Empty);

            foreach (var property in filePtrProperties)
            {
                AddOnAssetsImportedBlock(stringBuilder, property);
            }

            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine(string.Empty);
        }

        private static void AddOnAssetsImportedBlock(StringBuilder stringBuilder, Core.PropertyInfo property)
        {
            if (property.ContainerType == Core.ContainerType.StringMap)
            {
                stringBuilder.AppendLine($"            foreach (var path in this.{property.Name}Path)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("               UnityEngine.Object file = null;");
                stringBuilder.AppendLine("               tryGetAsset(path.Value, out file);");
                stringBuilder.AppendLine($"               this._{property.Name}.Add(path.Key, file);");
                stringBuilder.AppendLine("            }");
            }
            else if (property.ContainerType == Core.ContainerType.StaticArray && property.Container.ArraySize == 1)
            {
                stringBuilder.AppendLine($"            tryGetAsset(this.{property.Name}Path, out this._{property.Name});");
            }
            else
            {
                //stringBuilder.AppendLine($"            this._{property.Name} = (from path in this.{property.Name}Path select tryGetAsset(path)).ToList();");
                stringBuilder.AppendLine($"            this._{property.Name} = new List<UnityEngine.Object>();");
                stringBuilder.AppendLine($"            foreach (var path in this.{property.Name}Path)");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("               UnityEngine.Object file = null;");
                stringBuilder.AppendLine("               tryGetAsset(path, out file);");
                stringBuilder.AppendLine($"               this._{property.Name}.Add(file);");
                stringBuilder.AppendLine("            }");
            }
        }

        private static void AddMakeWritableStaticPropertiesFunction(StringBuilder stringBuilder, Core.PropertyInfo[] staticProperties)
        {
            stringBuilder.AppendLine("        /// <inheritdoc />");
            stringBuilder.AppendLine("        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);");

            foreach (var property in staticProperties)
            {
                MakeMakeWritableStaticPropertiesBlock(stringBuilder, property);
            }

            stringBuilder.AppendLine("            return parentProperties;");
            stringBuilder.AppendLine("        }");
        }

        private static void MakeMakeWritableStaticPropertiesBlock(
            StringBuilder stringBuilder,
            Core.PropertyInfo property)
        {
            var propertyTypeString = "Core.PropertyInfoType." + property.Type;
            var conversionFunctionString = GetUnityToFoxConversionFunctionString(property.Type);

            if (property.ContainerType == Core.ContainerType.StringMap)
            {
                stringBuilder.AppendLine($"            parentProperties.Add(PropertyInfoFactory.MakeStringMapProperty(\"{property.Name}\", {propertyTypeString}, this._{property.Name}.ToDictionary(entry => entry.Key, entry => {conversionFunctionString}(entry.Value) as object)));");
            }
            else if (property.ContainerType == Core.ContainerType.StaticArray && property.Container.ArraySize == 1)
            {
                stringBuilder.AppendLine($"            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty(\"{property.Name}\", {propertyTypeString}, {conversionFunctionString}(this._{property.Name})));");
            }
            else
            {
                var containerTypeString = "StaticArray";
                if (property.ContainerType == Core.ContainerType.DynamicArray)
                {
                    containerTypeString = "DynamicArray";
                }
                else if (property.ContainerType == Core.ContainerType.DynamicArray)
                {
                    containerTypeString = "List";
                }

                stringBuilder.AppendLine($"            parentProperties.Add(PropertyInfoFactory.Make{containerTypeString}Property(\"{property.Name}\", {propertyTypeString}, (from propertyEntry in this._{property.Name} select {conversionFunctionString}(propertyEntry) as object).ToArray()));");
            }
        }

        private static string GetFoxToUnityConversionFunctionString(Core.PropertyInfoType propertyType)
        {
            var conversionFunctionString = string.Empty;
            switch (propertyType)
            {
                case Core.PropertyInfoType.Color:
                    conversionFunctionString = "FoxUtils.FoxColorRGBAToUnityColor";
                    break;
                case Core.PropertyInfoType.EntityHandle:
                    conversionFunctionString = "initFunctions.GetEntityFromAddress";
                    break;
                case Core.PropertyInfoType.EntityPtr:
                    conversionFunctionString = "initFunctions.GetEntityFromAddress";
                    break;
                case Core.PropertyInfoType.FilePtr:
                    // TODO: Should this be in FoxUtils?
                    conversionFunctionString = "DataSetUtils.ExtractFilePath";
                    break;
                case Core.PropertyInfoType.Matrix3:
                    conversionFunctionString = "FoxUtils.FoxToUnity";
                    break;
                case Core.PropertyInfoType.Matrix4:
                    conversionFunctionString = "FoxUtils.FoxToUnity";
                    break;
                case Core.PropertyInfoType.Path:
                    conversionFunctionString = "DataSetUtils.ExtractFilePath";
                    break;
                case Core.PropertyInfoType.Quat:
                    conversionFunctionString = "FoxUtils.FoxToUnity";
                    break;
                case Core.PropertyInfoType.Vector3:
                    conversionFunctionString = "FoxUtils.FoxToUnity";
                    break;
                case Core.PropertyInfoType.Vector4:
                    conversionFunctionString = "FoxUtils.FoxToUnity";
                    break;
                case Core.PropertyInfoType.EntityLink:
                    conversionFunctionString = "initFunctions.MakeEntityLink";
                    break;
            }

            return conversionFunctionString;
        }

        private static string GetUnityToFoxConversionFunctionString(Core.PropertyInfoType propertyType)
        {
            var conversionFunctionString = string.Empty;
            switch (propertyType)
            {
                case Core.PropertyInfoType.Color:
                    conversionFunctionString = "FoxUtils.UnityColorToFoxColorRGBA";
                    break;
                case Core.PropertyInfoType.EntityHandle:
                    conversionFunctionString = "getEntityAddress";
                    break;
                case Core.PropertyInfoType.EntityPtr:
                    conversionFunctionString = "getEntityAddress";
                    break;
                case Core.PropertyInfoType.FilePtr:
                    // TODO: Should this be in FoxUtils?
                    conversionFunctionString = "DataSetUtils.AssetToFoxPath";
                    break;
                case Core.PropertyInfoType.Matrix3:
                    conversionFunctionString = "FoxUtils.UnityToFox";
                    break;
                case Core.PropertyInfoType.Matrix4:
                    conversionFunctionString = "FoxUtils.UnityToFox";
                    break;
                case Core.PropertyInfoType.Path:
                    conversionFunctionString = "DataSetUtils.ExtractFilePath";
                    break;
                case Core.PropertyInfoType.Quat:
                    conversionFunctionString = "FoxUtils.UnityToFox";
                    break;
                case Core.PropertyInfoType.Vector3:
                    conversionFunctionString = "FoxUtils.UnityToFox";
                    break;
                case Core.PropertyInfoType.Vector4:
                    conversionFunctionString = "FoxUtils.UnityToFox";
                    break;
                case Core.PropertyInfoType.EntityLink:
                    conversionFunctionString = "convertEntityLink";
                    break;
            }

            return conversionFunctionString;
        }

        private static IEnumerable<Core.PropertyInfo> GetPrunedStaticPropertyFields(
            IEnumerable<Core.PropertyInfo> staticProperties,
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
            // Initialize non-collection string properties to string.Empty; otherwise they default to null.
            var endOfLine = ";";
            if (property.ContainerType == Core.ContainerType.StaticArray && property.Container.ArraySize == 1)
            {
                if (property.Type == Core.PropertyInfoType.Path || property.Type == Core.PropertyInfoType.String)
                {
                    endOfLine = " = string.Empty;";
                }
            }

            stringBuilder.AppendLine($"        [SerializeField, Modules.DataSet.Property(\"{className}\")]");
            stringBuilder.AppendLine($"        private {GetPropertyFieldTypeString(property.ContainerType, property.Type, property.Container)} _{property.Name}{endOfLine}");
            
            if (property.Type != Core.PropertyInfoType.FilePtr && property.Type != Core.PropertyInfoType.Path)
            {
                return;
            }

            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("        [SerializeField, HideInInspector]");
            stringBuilder.AppendLine($"        private {GetPropertyFieldTypeString(property.ContainerType, Core.PropertyInfoType.String, property.Container)} {property.Name}Path;");
        }
        
        private static string GetPropertyFieldTypeString(
            Core.ContainerType containerType,
            Core.PropertyInfoType propertyType,
            Core.IContainer container)
        {
            var innerTypeString = GetConvertedTypeString(propertyType);
            
            if (containerType == Core.ContainerType.StaticArray && container.ArraySize == 1)
            {
                return innerTypeString;
            }

            if (containerType != Core.ContainerType.StringMap)
            {
                return $"List<{innerTypeString}>";
            }
            
            return $"OrderedDictionary_string_{GetConvertedTypeString(propertyType, false)}";
        }

        private static string GetConvertedTypeString(Core.PropertyInfoType type, bool addPrefix = true)
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
                    return "Entity";
                case Core.PropertyInfoType.Vector3:
                    return addPrefix ? "UnityEngine.Vector3" : "Vector3";
                case Core.PropertyInfoType.Vector4:
                    return addPrefix ? "UnityEngine.Vector4" : "Vector4";
                case Core.PropertyInfoType.Quat:
                    return addPrefix ? "UnityEngine.Quaternion" : "Quaternion";
                case Core.PropertyInfoType.Matrix3:
                    return addPrefix ? "UnityEngine.Matrix3x3" : "Matrix3x3";
                case Core.PropertyInfoType.Matrix4:
                    return addPrefix ? "UnityEngine.Matrix4x4" : "Matrix4x4";
                case Core.PropertyInfoType.Color:
                    return addPrefix ? "UnityEngine.Color" : "Color";
                case Core.PropertyInfoType.FilePtr:
                    return addPrefix ? "UnityEngine.Object" : "Object";
                case Core.PropertyInfoType.EntityHandle:
                    return "Entity";
                case Core.PropertyInfoType.EntityLink:
                    return addPrefix ? "FoxCore.EntityLink" : "EntityLink";
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

        private static string GetRawTypeString(Core.PropertyInfoType type)
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
                    return "ulong";
                case Core.PropertyInfoType.Vector3:
                    return "Core.Vector3";
                case Core.PropertyInfoType.Vector4:
                    return "Core.Vector4";
                case Core.PropertyInfoType.Quat:
                    return "Core.Quaternion";
                case Core.PropertyInfoType.Matrix3:
                    return "Core.Matrix3";
                case Core.PropertyInfoType.Matrix4:
                    return "Core.Matrix4";
                case Core.PropertyInfoType.Color:
                    return "Core.ColorRGBA";
                case Core.PropertyInfoType.FilePtr:
                    return "string";
                case Core.PropertyInfoType.EntityHandle:
                    return "ulong";
                case Core.PropertyInfoType.EntityLink:
                    return "Core.EntityLink";
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
            stringBuilder.AppendLine(MakeUsingStatement("System.Linq"));
            stringBuilder.AppendLine(MakeUsingStatement("System.Collections.Generic"));
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(MakeUsingStatement("FoxKit.Modules.DataSet.Exporter"));
            stringBuilder.AppendLine(MakeUsingStatement("FoxKit.Modules.DataSet.FoxCore"));
            stringBuilder.AppendLine(MakeUsingStatement("FoxKit.Utils"));
            stringBuilder.AppendLine(MakeUsingStatement("FoxKit.Utils.UI.StringMap"));
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(MakeUsingStatement("FoxLib"));
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(MakeUsingStatement("NUnit.Framework"));
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