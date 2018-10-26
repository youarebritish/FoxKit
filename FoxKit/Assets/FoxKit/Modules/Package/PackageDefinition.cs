namespace FoxKit.Modules.Archive
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    [CreateAssetMenu(menuName = "FoxKit/Package Definition", fileName = "New Package Definition", order = 4)]
    public class PackageDefinition : ScriptableObject
    {
        public enum PackageType
        {
            Fpk,
            Fpkd,
            Dat,
            Pftxs,
            Sbp
        }

        public PackageType Type = PackageType.Fpk;

        /// <summary>
        /// Get all of the assets contained within this package.
        /// </summary>
        /// <returns>
        /// The assets contained within the package.
        /// </returns>
        public List<UnityEngine.Object> GetContents()
        {
            // TODO: Support different PackageTypes only returning certain files (e.g., only get fox2s in fpkds)
            var assetFolderPath = AssetDatabase.GetAssetPath(this);
            var dataPath = Application.dataPath;
            var folderPath = dataPath.Substring(0, dataPath.Length - 6) + assetFolderPath;

            // Get the system file paths of all the files in the asset folder.
            var filePaths = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

            // Enumerate through the list of files loading the assets they represent and getting their type.
            return (from filePath in filePaths
                    where !filePath.EndsWith(".meta")
                    select filePath.Substring(dataPath.Length - 6)
                    into assetPath
                    select AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object))
                 into objAsset
                 where objAsset != null
                 select objAsset).ToList();
        }
    }
}