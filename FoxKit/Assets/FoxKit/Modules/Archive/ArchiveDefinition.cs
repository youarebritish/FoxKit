namespace FoxKit.Modules.Archive
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "FoxKit/Archive Definition", fileName = "New Archive Definition", order = 4)]
    public class ArchiveDefinition : ScriptableObject
    {
        public enum ArchiveType
        {
            Fpk,
            Fpkd,
            Dat,
            Pftxs,
            Sbp
        }

        public ArchiveType Type = ArchiveType.Fpk;
    }
}