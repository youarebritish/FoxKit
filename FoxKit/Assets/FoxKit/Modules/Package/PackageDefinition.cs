namespace FoxKit.Modules.Archive
{
    using System.Collections.Generic;

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

        public List<UnityEngine.Object> Entries = new List<Object>();

        public bool IsReadOnly;

        public void AssignEntries(List<UnityEngine.Object> entries, string guid)
        {
            this.Entries = entries;
            foreach (var entry in this.Entries)
            {
                var dataSet = entry as DataSetAsset;
                if (dataSet == null)
                {
                    continue;
                }

                dataSet.PackageGuid = guid;
            }
        }
    }
}