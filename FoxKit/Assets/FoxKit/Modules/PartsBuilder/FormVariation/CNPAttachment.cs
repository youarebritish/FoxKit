namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using System;
    using FoxKit.Core.WIP;
    using OdinSerializer;

    /// <summary>
    /// A form variation operation used by the Fox Engine used to attach one model, and, optionally, its auxilliary files, to another via a connection point (CNP).
    /// </summary>
    [System.Serializable]
    public class CNPAttachment
    {
        [OdinSerialize]
        public Str32CodeHashPair CNPName = null;

        [OdinSerialize]
        public PathFileNameCode64HashPair ModelFileName = null;

        [OdinSerialize]
        public PathFileNameCode64HashPair FrdvFileName = null;

        [OdinSerialize]
        public PathFileNameCode64HashPair SimFileName = null;

        /// <summary>
        /// #NEW#
        /// </summary>
        public CNPAttachment()
        {

        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public CNPAttachment(Str32CodeHashPair CNPName, PathFileNameCode64HashPair modelFileName, PathFileNameCode64HashPair frdvFileName, PathFileNameCode64HashPair simFileName)
        {
            this.CNPName = CNPName;

            this.ModelFileName = modelFileName;

            this.FrdvFileName = frdvFileName;

            this.SimFileName = simFileName;
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public static CNPAttachment Convert(FoxLib.FormVariation.CNPAttachment boneAttachment, Func<uint, string> str32DictFunc, Func<ulong, string> str64DictFunc)
        {
            Str32CodeHashPair CNPName = boneAttachment.CNPHash;
            CNPName.TryUnhashString(str32DictFunc);

            PathFileNameCode64HashPair modelFileName = boneAttachment.ModelFileHash;
            modelFileName.TryUnhashString(str64DictFunc);

            PathFileNameCode64HashPair frdvFileName = boneAttachment.FrdvFileHash;
            if (frdvFileName != null)
                frdvFileName.TryUnhashString(str64DictFunc);

            PathFileNameCode64HashPair simFileName = boneAttachment.SimFileHash;
            if (simFileName != null)
                simFileName.TryUnhashString(str64DictFunc);

            return new CNPAttachment(CNPName, modelFileName, frdvFileName, simFileName);
        }

        /// <summary>
        /// #NEW#
        /// </summary>
        public FoxLib.FormVariation.CNPAttachment Convert()
        {
            return new FoxLib.FormVariation.CNPAttachment(this.CNPName, this.ModelFileName, this.FrdvFileName, this.SimFileName);
        }
    }
}