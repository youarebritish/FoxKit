namespace FoxKit.Modules.DataSet.Importer
{
    using System.IO;
    using System.Text;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

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
            stringBuilder.AppendLine("namespace FoxKit.Modules.DataSet");

            // Open namespace block.
            stringBuilder.AppendLine("{");

            // Add using statements.
            stringBuilder.AppendLine("    using System;");
            stringBuilder.AppendLine("    using System.Collections.Generic;");
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("    using FoxKit.Modules.DataSet.Exporter;");
            stringBuilder.AppendLine("    using FoxKit.Modules.DataSet.FoxCore;");
            stringBuilder.AppendLine("    using FoxKit.Utils;");
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("    using FoxLib;");
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("    using UnityEditor;");
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("    using UnityEngine;");
            stringBuilder.AppendLine(string.Empty);

            stringBuilder.AppendLine("    /// <inheritdoc />");
            stringBuilder.AppendLine("    [Serializable]");

            // TODO Determine superclass
            stringBuilder.AppendLine($"    public class {entity.ClassName} : Data");

            // Open class block.
            stringBuilder.AppendLine("    {");

            // TODO fields
            stringBuilder.AppendLine("        /// <inheritdoc />");
            stringBuilder.AppendLine($"        public override short ClassId => {entity.ClassId};");
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine("        /// <inheritdoc />");
            stringBuilder.AppendLine($"        public override ushort Version => {entity.Version};");

            // TODO MakeWritableStaticProperties
            // TODO ReadProperty
            // TODO OnAssetsImported (if there are files referenced)

            // Close class block.
            stringBuilder.AppendLine("    }");

            // Close namespace block.
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
    }
}