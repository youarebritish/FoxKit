namespace FoxKit.Modules.Archive
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    [CreateAssetMenu(menuName = "FoxKit/Package Definition", fileName = "New Package Definition", order = 0)]
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

        public PackageType Type = PackageType.Fpkd;

        public List<UnityEngine.Object> Entries = new List<UnityEngine.Object>();

        public bool IsReadOnly;

        /// <summary>
        /// File paths of assets that this package wants references to.
        /// </summary>
        /// <remarks>
        /// This is a stupid workaround to the fact that when the package is imported, references to the files it imports are invalid.
        /// </remarks>
        [SerializeField, HideInInspector]
        private List<string> desiredAssets;

        /// <summary>
        /// Searches for this PackageDefinition's desiredAssets and assigns their references.
        /// </summary>
        public void AssignEntries()
        {
            if (this.desiredAssets == null)
            {
                return;
            }

            var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));

            foreach (var entry in this.desiredAssets)
            {
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(entry);
                if (asset == null)
                {
                    continue;
                }

                this.Entries.Add(asset);

                if (!(asset is EntityFileAsset))
                {
                    continue;
                }

                ((EntityFileAsset)asset).PackageGuid = guid;
            }

            this.desiredAssets.Clear();
        }
        
        /// <summary>
        /// Assign the paths to the files this PackageDefinition wants to reference. Should only be called when importing the package.
        /// </summary>
        /// <param name="paths">The paths of the assets to reference.</param>
        public void AssignDesiredFiles(IEnumerable<string> paths)
        {
            this.desiredAssets = paths.ToList();
        }
    }
}