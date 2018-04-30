namespace FoxKit.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Provides helper functions related to hashing.
    /// </summary>
    public static class Hashing
    {
        /// <summary>
        /// MD5 hash algorithm instance.
        /// </summary>
        private static readonly MD5 Md5 = MD5.Create();

        /// <summary>
        /// Dictionary of hash names.
        /// </summary>
        private static readonly Dictionary<ulong, string> HashNameDictionary = new Dictionary<ulong, string>();

        /// <summary>
        /// Supported file extensions.
        /// </summary>
        /// <remarks>TODO: Maybe move this out into a file?</remarks>
        private static readonly List<string> FileExtensions = new List<string>
    {
        "1.ftexs",
        "1.nav2",
        "2.ftexs",
        "3.ftexs",
        "4.ftexs",
        "5.ftexs",
        "6.ftexs",
        "ag.evf",
        "aia",
        "aib",
        "aibc",
        "aig",
        "aigc",
        "aim",
        "aip",
        "ait",
        "atsh",
        "bnd",
        "bnk",
        "cc.evf",
        "clo",
        "csnav",
        "dat",
        "des",
        "dnav",
        "dnav2",
        "eng.lng",
        "ese",
        "evb",
        "evf",
        "fag",
        "fage",
        "fago",
        "fagp",
        "fagx",
        "fclo",
        "fcnp",
        "fcnpx",
        "fdes",
        "fdmg",
        "ffnt",
        "fmdl",
        "fmdlb",
        "fmtt",
        "fnt",
        "fova",
        "fox",
        "fox2",
        "fpk",
        "fpkd",
        "fpkl",
        "frdv",
        "fre.lng",
        "frig",
        "frt",
        "fsd",
        "fsm",
        "fsml",
        "fsop",
        "fstb",
        "ftex",
        "fv2",
        "fx.evf",
        "fxp",
        "gani",
        "geom",
        "ger.lng",
        "gpfp",
        "grxla",
        "grxoc",
        "gskl",
        "htre",
        "info",
        "ita.lng",
        "jpn.lng",
        "json",
        "lad",
        "ladb",
        "lani",
        "las",
        "lba",
        "lng",
        "lpsh",
        "lua",
        "mas",
        "mbl",
        "mog",
        "mtar",
        "mtl",
        "nav2",
        "nta",
        "obr",
        "obrb",
        "parts",
        "path",
        "pftxs",
        "ph",
        "phep",
        "phsd",
        "por.lng",
        "qar",
        "rbs",
        "rdb",
        "rdf",
        "rnav",
        "rus.lng",
        "sad",
        "sand",
        "sani",
        "sbp",
        "sd.evf",
        "sdf",
        "sim",
        "simep",
        "snav",
        "spa.lng",
        "spch",
        "sub",
        "subp",
        "tgt",
        "tre2",
        "txt",
        "uia",
        "uif",
        "uig",
        "uigb",
        "uil",
        "uilb",
        "utxl",
        "veh",
        "vfx",
        "vfxbin",
        "vfxdb",
        "vnav",
        "vo.evf",
        "vpc",
        "wem",
        "xml"
    };

        /// <summary>
        /// Map of supported extensions.
        /// </summary>
        private static readonly Dictionary<ulong, string> ExtensionsMap = FileExtensions.ToDictionary(HashFileExtension);

        /// <summary>
        /// Magic number
        /// </summary>
        public const ulong MetaFlag = 0x4000000000000;

        /// <summary>
        /// Hash a file extension.
        /// </summary>
        /// <param name="fileExtension">The extension to hash.</param>
        /// <returns>The hashed extension.</returns>
        private static ulong HashFileExtension(string fileExtension)
        {
            return HashFileName(fileExtension, false) & 0x1FFF;
        }

        /// <summary>
        /// Hash a filename.
        /// </summary>
        /// <param name="text">Filename to hash.</param>
        /// <param name="removeExtension">Whether or not to remove the extension.</param>
        /// <returns>The hashed filename.</returns>
        public static ulong HashFileName(string text, bool removeExtension = true)
        {
            if (removeExtension)
            {
                int index = text.IndexOf('.');
                text = index == -1 ? text : text.Substring(0, index);
            }

            bool metaFlag = false;
            const string assetsConstant = "/Assets/";
            if (text.StartsWith(assetsConstant))
            {
                text = text.Substring(assetsConstant.Length);

                if (text.StartsWith("tpptest"))
                {
                    metaFlag = true;
                }
            }
            else
            {
                metaFlag = true;
            }

            text = text.TrimStart('/');

            const ulong seed0 = 0x9ae16a3b2f90404f;
            byte[] seed1Bytes = new byte[sizeof(ulong)];
            for (int i = text.Length - 1, j = 0; i >= 0 && j < sizeof(ulong); i--, j++)
            {
                seed1Bytes[j] = Convert.ToByte(text[i]);
            }
            ulong seed1 = BitConverter.ToUInt64(seed1Bytes, 0);
            ulong maskedHash = CityHash.CityHash.CityHash64WithSeeds(text, seed0, seed1) & 0x3FFFFFFFFFFFF;

            return metaFlag
                ? maskedHash | MetaFlag
                : maskedHash;
        }

        /// <summary>
        /// Hash a filename with legacy hashing algorithm.
        /// </summary>
        /// <param name="text">Filename to hash.</param>
        /// <param name="removeExtension">Whether or not to remove the extension.</param>
        /// <returns>The legacy-hashed filename.</returns>
        public static ulong HashFileNameLegacy(string text, bool removeExtension = true)
        {
            if (removeExtension)
            {
                int index = text.IndexOf('.');
                text = index == -1 ? text : text.Substring(0, index);
            }

            const ulong seed0 = 0x9ae16a3b2f90404f;
            ulong seed1 = text.Length > 0 ? (uint)((text[0]) << 16) + (uint)text.Length : 0;
            return CityHash.CityHash.CityHash64WithSeeds(text + "\0", seed0, seed1) & 0xFFFFFFFFFFFF;
        }

        /// <summary>
        /// Hash a filename with extension.
        /// </summary>
        /// <param name="filePath">Filename to hash.</param>
        /// <returns>The hashed filename.</returns>
        public static ulong HashFileNameWithExtension(string filePath)
        {
            filePath = DenormalizeFilePath(filePath);
            string hashablePart;
            string extensionPart;
            int extensionIndex = filePath.IndexOf(".", StringComparison.Ordinal);
            if (extensionIndex == -1)
            {
                hashablePart = filePath;
                extensionPart = "";
            }
            else
            {
                hashablePart = filePath.Substring(0, extensionIndex);
                extensionPart = filePath.Substring(extensionIndex + 1, filePath.Length - extensionIndex - 1);
            }

            ulong typeId = 0;
            var extensions = ExtensionsMap.Where(e => e.Value == extensionPart).ToList();
            if (extensions.Count == 1)
            {
                var extension = extensions.Single();
                typeId = extension.Key;
            }
            ulong hash = HashFileName(hashablePart);
            hash = (typeId << 51) | hash;
            return hash;
        }

        /// <summary>
        /// Replace /'s with \\'s and remove leading occurrence of \\.
        /// </summary>
        /// <param name="filePath">The filename to normalize.</param>
        /// <returns>The normalized filename.</returns>
        internal static string NormalizeFilePath(string filePath)
        {
            return filePath.Replace("/", "\\").TrimStart('\\');
        }

        /// <summary>
        /// Replace \\'s with /'s.
        /// </summary>
        /// <param name="filePath">The filename to denormalize.</param>
        /// <returns>The denormalized filename.</returns>
        private static string DenormalizeFilePath(string filePath)
        {
            return filePath.Replace("\\", "/");
        }

        /// <summary>
        /// Try to get a filename with a given hash.
        /// </summary>
        /// <param name="hash">The hashed filename.</param>
        /// <param name="fileName">The recovered filename.</param>
        /// <returns>True if the filename was found.</returns>
        internal static bool TryGetFileNameFromHash(ulong hash, out string fileName)
        {
            bool foundFileName = true;
            string filePath;
            string fileExtension;

            ulong extensionHash = hash >> 51;
            ulong pathHash = hash & 0x3FFFFFFFFFFFF;

            fileName = "";
            if (!HashNameDictionary.TryGetValue(pathHash, out filePath))
            {
                filePath = pathHash.ToString("x");
                foundFileName = false;
            }

            fileName += filePath;

            if (!ExtensionsMap.TryGetValue(extensionHash, out fileExtension))
            {
                fileExtension = "_unknown";
                foundFileName = false;
            }
            else
            {
                fileName += ".";
            }
            fileName += fileExtension;
            
            return foundFileName;
        }

        /// <summary>
        /// Compute the MD5 hash for a byte array.
        /// </summary>
        /// <param name="buffer">Byte array to hash.</param>
        /// <returns>The MD5 hash.</returns>
        internal static byte[] Md5Hash(byte[] buffer)
        {
            return Md5.ComputeHash(buffer);
        }

        /// <summary>
        /// Compute the MD5 hash for a string.
        /// </summary>
        /// <param name="text">String to hash.</param>
        /// <returns>The MD5 hash.</returns>
        internal static byte[] Md5HashText(string text)
        {
            return Md5.ComputeHash(Encoding.Default.GetBytes(text));
        }
    }
}