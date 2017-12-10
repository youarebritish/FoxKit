using System;
using System.Collections.Generic;
using System.IO;
using FtexTool.Ftex.Enum;

namespace FtexTool
{
    internal class FtexToolArguments
    {
        public FtexToolArguments()
        {
            Errors = new List<string>();
        }

        public bool DisplayHelp { get; set; }

        public FtexTextureType TextureType { get; set; }

        public FtexUnknownFlags UnknownFlags { get; set; }

        public string InputPath { get; set; }

        public bool DirectoryInput { get; set; }

        public string OutputPath { get; set; }

        public int? FtexsFileCount { get; set; }

        public List<string> Errors { get; }

        public static FtexToolArguments Parse(string[] args)
        {
            FtexToolArguments arguments = new FtexToolArguments
            {
                DisplayHelp = false,
                TextureType = FtexTextureType.DiffuseMap,
                UnknownFlags = FtexUnknownFlags.Default,
                InputPath = "",
                OutputPath = ""
            };
            if (args.Length == 0)
            {
                arguments.DisplayHelp = true;
                return arguments;
            }

            bool expectType = false;
            bool expectUnknownFlags = false;
            bool expectFtexs = false;
            bool expectInput = false;
            bool expectOutput = false;

            int argIndex = 0;
            while (argIndex < args.Length)
            {
                string arg = args[argIndex];
                argIndex++;
                if (expectType)
                {
                    arguments.ReadType(arg);
                    expectType = false;
                }
                else if (expectFtexs)
                {
                    arguments.ReadFtexsCount(arg);
                    expectFtexs = false;
                }
                else if (expectUnknownFlags)
                {
                    arguments.ReadUnknownFlags(arg);
                    expectUnknownFlags = false;
                }
                else if (expectInput)
                {
                    arguments.ReadInput(arg);
                    expectInput = false;
                }
                else if (expectOutput)
                {
                    arguments.ReadOutput(arg);
                    expectOutput = false;
                }
                else if (arg.StartsWith("-"))
                {
                    switch (arg)
                    {
                        case "-h":
                        case "-help":
                            arguments.DisplayHelp = true;
                            break;
                        case "-t":
                        case "-type":
                            expectType = true;
                            break;
                        case "-fl":
                        case "-flags":
                            expectUnknownFlags = true;
                            break;
                        case "-f":
                        case "-ftexs":
                            expectFtexs = true;
                            break;
                        case "-i":
                        case "-input":
                            expectInput = true;
                            break;
                        case "-o":
                        case "-output":
                            expectOutput = true;
                            break;
                        default:
                            arguments.Errors.Add("Unknown option");
                            break;
                    }
                }
                else
                {
                    expectInput = true;
                    expectOutput = true;
                    argIndex--;
                }
            }
            return arguments;
        }

        private void ReadUnknownFlags(string flagArg)
        {
            FtexUnknownFlags flags;
            if (Enum.TryParse<FtexUnknownFlags>(flagArg, out flags))
            {
                UnknownFlags = flags;
                return;
            }

            short flagsValue;
            if (short.TryParse(flagArg, out flagsValue))
            {
                UnknownFlags = (FtexUnknownFlags)flagsValue;
                return;
            }

            Errors.Add($"{flagArg} is not a valid flag.");
        }

        public void ReadType(string type)
        {
            switch (type)
            {
                case "d":
                case "diffuse":
                    TextureType = FtexTextureType.DiffuseMap;
                    break;
                case "m":
                case "material":
                    TextureType = FtexTextureType.MaterialMap;
                    break;
                case "n":
                case "normal":
                    TextureType = FtexTextureType.NormalMap;
                    break;
                case "c":
                case "cube":
                    TextureType = FtexTextureType.CubeMap;
                    break;
                default:
                    Errors.Add($"{type} is not a valid texture type.");
                    break;
            }
        }

        public void ReadInput(string inputPath)
        {
            InputPath = inputPath;
            try
            {
                FileAttributes attributes = File.GetAttributes(inputPath);
                DirectoryInput = (attributes & FileAttributes.Directory) == FileAttributes.Directory;
            }
            catch (Exception)
            {
                Errors.Add($"File/Directory {inputPath} not found");
            }
        }

        public void ReadOutput(string outputPath)
        {
            OutputPath = outputPath;
        }

        public void ReadFtexsCount(string ftexsFileCount)
        {
            int count;
            if (int.TryParse(ftexsFileCount, out count) && count >= 0)
            {
                FtexsFileCount = count;
            }
            else
            {
                Errors.Add($"Invalid ftexs file count {ftexsFileCount}");
            }
        }
    }
}
