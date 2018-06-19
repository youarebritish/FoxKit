namespace FoxKit.Modules.DataSet.Importer
{
    using System.Text;

    using FoxLib;

    public static class ClassGenerator
    {
        public static void GenerateClassFromEntity(Core.Entity entity)
        {
            var sourceCode = GenerateClassSourceCode(entity);
            System.IO.File.WriteAllText($"{entity.ClassName}.cs", sourceCode);
        }

        private static string GenerateClassSourceCode(Core.Entity entity)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("namespace FoxKit.Modules.DataSet");

            // Open namespace block.
            stringBuilder.AppendLine("{");

            // TODO Usings
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
            stringBuilder.AppendLine($"        public override short Version => {entity.Version};");

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