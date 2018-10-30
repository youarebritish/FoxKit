namespace FoxKit.Modules.Archive.Importer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using GzsTool.Core.Common;
    using GzsTool.Core.Fpk;
    using GzsTool.Core.Pftxs;
    using GzsTool.Core.Qar;
    using GzsTool.Core.Sbp;

    using UnityEditor;
    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;
    using UnityEngine.Assertions;

    [ScriptedImporter(1, new[] { "fpk", "fpkd", "dat", "pftxs", "sbp" })]
    public class PackageImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var extension = Path.GetExtension(ctx.assetPath);
            var archiveDefinition = ScriptableObject.CreateInstance<PackageDefinition>();
            archiveDefinition.name = Path.GetFileNameWithoutExtension(ctx.assetPath);
            archiveDefinition.IsReadOnly = true;
            Action<IEnumerable<UnityEngine.Object>> assignEntries = entries => archiveDefinition.AssignEntries(entries.ToList());

            // TODO Read dictionaries
            switch (extension)
            {
                case ".dat":
                    archiveDefinition.Type = PackageDefinition.PackageType.Dat;
                    ReadArchive<QarFile>(ctx.assetPath, assignEntries);
                    break;
                case ".fpk":
                    archiveDefinition.Type = PackageDefinition.PackageType.Fpk;
                    ReadArchive<FpkFile>(ctx.assetPath, assignEntries);
                    break;
                case ".fpkd":
                    archiveDefinition.Type = PackageDefinition.PackageType.Fpkd;
                    ReadArchive<FpkFile>(ctx.assetPath, assignEntries);
                    break;
                case ".pftxs":
                    archiveDefinition.Type = PackageDefinition.PackageType.Pftxs;
                    ReadArchive<PftxsFile>(ctx.assetPath, assignEntries);
                    break;
                case ".sbp":
                    archiveDefinition.Type = PackageDefinition.PackageType.Sbp;
                    ReadArchive<SbpFile>(ctx.assetPath, assignEntries);
                    break;
                default:
                    break;
            }

            ctx.AddObjectToAsset("definition", archiveDefinition);
            ctx.SetMainObject(archiveDefinition);
            
            /* TODO
             * Calling this currently triggers a (harmless?) "A default asset was created for 'blah.fpkd' because the asset importer crashed on it last time."
             * error message. However, removing it makes it so that Unity doesn't detect the new files immediately. Figure out what to do about this. */
            AssetDatabase.Refresh();
        }

        private static void ReadArchive<T>(string path, Action<IEnumerable<UnityEngine.Object>> assignEntries) where T : ArchiveFile, new()
        {
            Assert.IsFalse(string.IsNullOrEmpty(path));
            Assert.IsNotNull(assignEntries);

            var files = new List<string>();
            using (var input = new FileStream(path, FileMode.Open))
            {
                var file = new T { Name = Path.GetFileName(path) };
                file.Read(input);

                foreach (var exportedFile in file.ExportFiles(input))
                {
                    files.Add(exportedFile.FileName);

                    var outputDirectory = new FileSystemDirectory(Path.GetDirectoryName(exportedFile.FileName));
                    var filename = Path.GetFileName(exportedFile.FileName);
                    outputDirectory.WriteFile(filename, exportedFile.DataStream);
                }
            }

            AssetDatabase.Refresh();
            assignEntries(from file in files
                          select AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(file));
        }
    }
}