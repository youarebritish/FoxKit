using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEditor;

using UnityEngine;

public static class TextAssetExtensions
{
    public static bool ContainsLine(this TextAsset textAsset, string entry)
    {
        var linesInFile = textAsset.text.Split('\n');
        return linesInFile
            .Select(line => Regex.Replace(line, @"\t|\n|\r", string.Empty))
            .Any(lineWithoutNewLines => lineWithoutNewLines == entry);
    }

    public static void Overwrite(this TextAsset textAsset, IEnumerable<string> lines)
    {
        File.WriteAllLines(AssetDatabase.GetAssetPath(textAsset), lines);
    }

    public static void AppendLine(this TextAsset textAsset, string line)
    {
        File.AppendAllText(AssetDatabase.GetAssetPath(textAsset), line + Environment.NewLine);
    }

    public static ISet<string> GetUniqueLines(this TextAsset textAsset)
    {
        var linesInFile = textAsset.text.Split('\n');
        var linesInFileNoNewLines = linesInFile.Select(line => Regex.Replace(line, @"\t|\n|\r", string.Empty));
        return new HashSet<string>(linesInFileNoNewLines);
    }
}
