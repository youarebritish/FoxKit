using FoxKit.Core;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Imports and exports plaintext files.
/// </summary>
public class PlaintextHandler : IFormatHandler
{
    public List<string> Extensions => SupportedExtensions;

    private readonly List<string> SupportedExtensions = new List<string>() { "lua", "json", "dat" };

    public void Import(Stream input, string path)
    {
        using (var outputStream = File.Create(path))
        {
            input.CopyTo(outputStream);
        }
    }

    public void Export(object asset, string path)
    {
        throw new System.NotImplementedException();
    }
}
