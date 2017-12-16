namespace FoxKit.Modules.FormatHandlers.TextureHandler
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    using FoxKit.Core;
    using FoxKit.Modules.FormatHandlers.ArchiveHandler;

    using FtexTool;
    using FtexTool.Dds;
    using FtexTool.Exceptions;
    using FtexTool.Ftex;
    using FtexTool.Ftexs;

    using GzsTool.Core.Common;

    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// Imports and exports texture files.
    /// </summary>
    public class TextureHandler : IFormatHandler
    {
        /// <inheritdoc />
        // TODO: Maybe split up Extensions and DependentExtensions for files that can't be read on their own
        public List<string> Extensions { get; } = new List<string> { "ftex", "ftexs", "1.ftexs", "2.ftexs", "3.ftexs", "4.ftexs", "5.ftexs" };

        private readonly FileRegistry fileRegistry;

        public TextureHandler(FileRegistry fileRegistry)
        {
            this.fileRegistry = fileRegistry;
        }

        /// <inheritdoc />
        public object Import(Stream input, string path)
        {
            //UnityEngine.Debug.Log("Extracting " + path);
            var extension = FileRegistry.GetExtension(path);
            if (extension == "ftex")
            {
                UnpackFtexFile(input, path, path, fileRegistry);
            }
            return null;
        }

        private static void UnpackFtexFile(Stream input, string filePath, string outputPath, FileRegistry fileRegistry)
        {
            string fileDirectory = Path.GetDirectoryName(filePath);//string.IsNullOrEmpty(outputPath) ? Path.GetDirectoryName(filePath) ?? string.Empty : outputPath;
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            var ftexFile = GetFtexFile(input, filePath, fileRegistry);
            if (ftexFile == null)
            {
                return;
            }

            var ddsFile = FtexDdsConverter.ConvertToDds(ftexFile);

            string ddsFileName = $"{fileName}.dds";
            string ddsFilePath = Path.Combine(fileDirectory, ddsFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(ddsFilePath));

            using (var outputStream = new FileStream(ddsFilePath, FileMode.Create))
            {
                ddsFile.Write(outputStream);
            }
        }

        private static FtexFile GetFtexFile(Stream input, string filePath, FileRegistry fileRegistry)
        {
            var ftexFile = FtexFile.ReadFtexFile(input);

            for (byte fileNumber = 0; fileNumber <= ftexFile.FtexsFileCount; fileNumber++)
            {
                var ftexsFile = new FtexsFile
                {
                    FileNumber = fileNumber
                };
                ftexFile.AddFtexsFile(ftexsFile);
            }

            foreach (var mipMapInfo in ftexFile.MipMapInfos)
            {
                Stream ftexsStream;
                if (mipMapInfo.FtexsFileNumber == 0)
                {
                    ftexsStream = input;
                }
                else
                {
                    var ftexsContainer = FindFtexsDataStreamContainer(Path.GetFileNameWithoutExtension(filePath) + "." + mipMapInfo.FtexsFileNumber + ".ftexs", mipMapInfo.FtexsFileNumber, fileRegistry);

                    if (ftexsContainer == null)
                    {
                        UnityEngine.Debug.LogError($"Mip {mipMapInfo.FtexsFileNumber} for {filePath} not found");
                        return null;
                    }

                    UnityEngine.Debug.Log($"Found mip {mipMapInfo.FtexsFileNumber} for {filePath}");
                    ftexsStream = ftexsContainer.DataStream();
                }

                FtexsFile ftexsFile;
                if (ftexFile.TryGetFtexsFile(mipMapInfo.FtexsFileNumber, out ftexsFile))
                {
                    ReadFtexsFile(ftexsStream, ftexsFile, mipMapInfo);
                }
            }
            return ftexFile;
        }

        private static FileDataStreamContainer FindFtexsDataStreamContainer(string ftexFilename, byte ftexsFileNumber, FileRegistry fileRegistry)
        {
            Assert.IsTrue(ftexsFileNumber > 0, "Trying to look for mip 0 in an ftexs.");

            var ftexsExtension = $"{ftexsFileNumber}.ftexs";

            if (fileRegistry.ContainsExtension(ftexsExtension))
            {
                return fileRegistry.FindFile(ftexFilename, ftexsExtension);
            }
            else
            {
                UnityEngine.Debug.LogError("Extension not registered: " + ftexsExtension);
                return null;
            }
        }

        private static void ReadFtexsFile(Stream ftexsStream, FtexsFile ftexsFile, FtexFileMipMapInfo mipMapInfo)
        {
            ftexsStream.Position = mipMapInfo.Offset;
            ftexsFile.Read(
                ftexsStream,
                mipMapInfo.ChunkCount,
                mipMapInfo.Offset,
                mipMapInfo.DecompressedFileSize);
        }

        /// <inheritdoc />
        public void Export(object asset, string path)
        {
            throw new System.NotImplementedException();
        }
    }
}