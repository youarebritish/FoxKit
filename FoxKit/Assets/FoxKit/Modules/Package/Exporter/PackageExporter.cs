namespace FoxKit.Modules.Package.Exporter
{
    using System.IO;

    using FoxKit.Core;
    using FoxKit.Modules.Archive;

    using GzsTool.Core.Fpk;

    using UnityEditor;

    using UnityEngine;

    public static class PackageExporter
    {
        public static void Export(PackageDefinition package, Stream outputStream)
        {
            if (package.Type != PackageDefinition.PackageType.Fpk && package.Type != PackageDefinition.PackageType.Fpkd)
            {
                Debug.LogError("Only exporting fpk and fpkd packages is currently supported.");
                return;
            }

            var archiveFile = new FpkFile();
            archiveFile.FpkType = FpkType.Fpk;
            if (package.Type == PackageDefinition.PackageType.Fpkd)
            {
                archiveFile.FpkType = FpkType.Fpkd;
            }

            // TODO references
            foreach (var entry in package.Entries)
            {
                if (entry is IFoxAsset)
                {
                    // TODO convert
                    continue;
                }

                var fpkEntry = new FpkEntry { FilePath = AssetDatabase.GetAssetPath(entry) };
                archiveFile.Entries.Add(fpkEntry);
            }
        }
    }
}