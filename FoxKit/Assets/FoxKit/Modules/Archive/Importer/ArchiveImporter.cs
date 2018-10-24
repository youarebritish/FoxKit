namespace FoxKit.Modules.Archive.Importer
{
    using System.IO;

    using GzsTool.Core.Common;
    using GzsTool.Core.Fpk;
    using GzsTool.Core.Pftxs;
    using GzsTool.Core.Qar;
    using GzsTool.Core.Sbp;

    using UnityEditor;
    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;

    [ScriptedImporter(1, new[] { "fpk", "fpkd", "dat", "pftxs", "sbp" })]
    public class ArchiveImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var extension = Path.GetExtension(ctx.assetPath);
            var archiveDefinition = ScriptableObject.CreateInstance<ArchiveDefinition>();

            // TODO Read dictionaries
            switch (extension)
            {
                case ".dat":
                    archiveDefinition.Type = ArchiveDefinition.ArchiveType.Dat;
                    ReadArchive<QarFile>(ctx.assetPath);
                    break;
                case ".fpk":
                    archiveDefinition.Type = ArchiveDefinition.ArchiveType.Fpk;
                    ReadArchive<FpkFile>(ctx.assetPath);
                    break;
                case ".fpkd":
                    archiveDefinition.Type = ArchiveDefinition.ArchiveType.Fpkd;
                    ReadArchive<FpkFile>(ctx.assetPath);
                    break;
                case ".pftxs":
                    archiveDefinition.Type = ArchiveDefinition.ArchiveType.Pftxs;
                    ReadArchive<PftxsFile>(ctx.assetPath);
                    break;
                case ".sbp":
                    archiveDefinition.Type = ArchiveDefinition.ArchiveType.Sbp;
                    ReadArchive<SbpFile>(ctx.assetPath);
                    break;
            }

            ctx.AddObjectToAsset("definition", archiveDefinition);
            ctx.SetMainObject(archiveDefinition);
            
            /* TODO
             * Calling this currently triggers a (harmless?) "A default asset was created for 'blah.fpkd' because the asset importer crashed on it last time."
             * error message. However, removing it makes it so that Unity doesn't detect the new files immediately. Figure out what to do about this. */
            AssetDatabase.Refresh();
        }

        private static void ReadArchive<T>(string path) where T : ArchiveFile, new()
        {
            using (var input = new FileStream(path, FileMode.Open))
            {
                var file = new T { Name = Path.GetFileName(path) };
                file.Read(input);
                foreach (var exportedFile in file.ExportFiles(input))
                {
                    var outputDirectory = new FileSystemDirectory(Path.GetDirectoryName(exportedFile.FileName));
                    var filename = Path.GetFileName(exportedFile.FileName);
                    outputDirectory.WriteFile(filename, exportedFile.DataStream);
                }
            }
        }
    }
}