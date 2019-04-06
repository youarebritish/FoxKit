namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using System;
    using FoxKit.Core.WIP;
    using OdinSerializer;

    /// <summary>
    /// A form variation operation used by the Fox Engine used to attach one model, and, optionally, its auxilliary files, to another via bones.
    /// </summary>
    [System.Serializable]
    public class BoneAttachment
    {
        [OdinSerialize]
        public PathFileNameCode64HashPair ModelFileName = null;

        [OdinSerialize]
        public PathFileNameCode64HashPair FrdvFileName = null;

        [OdinSerialize]
        public PathFileNameCode64HashPair SimFileName = null;

        /// <summary>
        /// #NEW#
        /// </summary>
        public BoneAttachment()
        {

        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public BoneAttachment(PathFileNameCode64HashPair modelFileName, PathFileNameCode64HashPair frdvFileName, PathFileNameCode64HashPair simFileName)
        {
            this.ModelFileName = modelFileName;

            this.FrdvFileName = frdvFileName;

            this.SimFileName = simFileName;
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public static BoneAttachment Convert(FoxLib.FormVariation.BoneAttachment boneAttachment, Func<ulong, string> str64DictFunc)
        {
            PathFileNameCode64HashPair modelFileName = boneAttachment.ModelFileHash;
            modelFileName.TryUnhashString(str64DictFunc);

            PathFileNameCode64HashPair frdvFileName = boneAttachment.FrdvFileHash;
            if (frdvFileName != null)
                frdvFileName.TryUnhashString(str64DictFunc);

            PathFileNameCode64HashPair simFileName = boneAttachment.SimFileHash;
            if (simFileName != null)
                simFileName.TryUnhashString(str64DictFunc);

            return new BoneAttachment(modelFileName, frdvFileName, simFileName);
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public FoxLib.FormVariation.BoneAttachment Convert()
        {
            return new FoxLib.FormVariation.BoneAttachment(this.ModelFileName, this.FrdvFileName, this.SimFileName);
        }
    }
}