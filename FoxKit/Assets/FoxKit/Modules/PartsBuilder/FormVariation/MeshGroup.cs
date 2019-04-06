namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using System;
    using FoxKit.Core.WIP;
    using OdinSerializer;

    /// <summary>
    /// A form variation operation used by the Fox Engine used to show a given mesh group.
    /// </summary>
    [System.Serializable]
    public class MeshGroup
    {
        [OdinSerialize]
        public Str32CodeHashPair MeshGroupName;

        /// <summary>
        /// #NEW#
        /// </summary>
        public MeshGroup()
        {
            this.MeshGroupName = null;
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public MeshGroup(Str32CodeHashPair meshGroupName)
        {
            this.MeshGroupName = meshGroupName;
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public static MeshGroup Convert(Str32CodeHashPair meshGroupName, Func<uint, string> dictFunc)
        {
            return new MeshGroup(dictFunc(meshGroupName));
        }
    }
}