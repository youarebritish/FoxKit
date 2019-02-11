using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FoxKit.Core.WIP
{
    public static class Dictionaries
    {
        public static StrCode32HashManager FV2StringDictionary = new StrCode32HashManager();
        public static PathFileNameCode64HashManager FV2FileStringDictionary = new PathFileNameCode64HashManager();

        public static Dictionary<string, uint> Str32CodeHashDictionary = new Dictionary<string, uint>();
        public static Dictionary<string, ulong> PathFileNameCode64HashDictionary = new Dictionary<string, ulong>();

        static Dictionaries()
        {
            FV2StringDictionary.LoadDictionary(AssetDatabase.LoadAssetAtPath<TextAsset>("Assets\\Config\\Dictionaries\\fpk_dictionary.txt"));
            FV2FileStringDictionary.LoadDictionary(AssetDatabase.LoadAssetAtPath<TextAsset>("Assets\\Config\\Dictionaries\\fpk_file_dictionary.txt"));

            Str32CodeHashDictionary.Add("0x5E6774A", 98989898);
        }
    }
}