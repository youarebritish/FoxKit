namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using FoxKit.Core;

    /// <summary>
    /// A form variation operation used by the Fox Engine used to attach one model, and, optionally, its auxilliary files, to another via bones.
    /// </summary>
    [System.Serializable]
    public struct BoneAttachment
    {
        public StrCode64StringPair ModelFileName;
        public StrCode64StringPair FrdvFileName;
        public StrCode64StringPair SimFileName;

        /// <summary>
        /// Initializes a new instance of the BoneAttachment struct.
        /// </summary>
        /// <param name="modelFileName">Model file name.</param>
        /// <param name="frdvFileName">Frdv file name.</param>
        /// <param name="simFileName">Sim file name.</param>
        public BoneAttachment(StrCode64StringPair modelFileName, StrCode64StringPair frdvFileName, StrCode64StringPair simFileName)
        {
            this.ModelFileName = modelFileName;

            this.FrdvFileName = frdvFileName;

            this.SimFileName = simFileName;
        }

        /// <summary>
        /// Creates a FoxKit BoneAttachment from a given FoxLib BoneAttachment.
        /// </summary>
        /// <param name="boneAttachment">The FoxLib BoneAttachment.</param>
        /// <param name="fileHashManager">An StrCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxKit BoneAttachment.</returns>
        public static BoneAttachment MakeFoxKitBoneAttachment(FoxLib.FormVariation.BoneAttachment boneAttachment, StrCode64HashManager fileHashManager)
        {
            ulong modelFileHash = boneAttachment.ModelFileHash;
            ulong? frdvFileHash = boneAttachment.FrdvFileHash;
            ulong? simFileHash = boneAttachment.SimFileHash;

            StrCode64StringPair modelFileName;
            StrCode64StringPair frdvFileName;
            StrCode64StringPair simFileName;

            modelFileName = fileHashManager.GetStringPairFromUnhashAttempt(modelFileHash);

            if (frdvFileHash != null)
            {
                frdvFileName = fileHashManager.GetStringPairFromUnhashAttempt(frdvFileHash.Value);
            }
            else
            {
                frdvFileName = new StrCode64StringPair(string.Empty);
            }

            if (simFileHash != null)
            {
                simFileName = fileHashManager.GetStringPairFromUnhashAttempt(simFileHash.Value);
            }
            else
            {
                simFileName = new StrCode64StringPair(string.Empty);
            }

            return new BoneAttachment(modelFileName, frdvFileName, simFileName);
        }

        /// <summary>
        /// Creates a FoxLib BoneAttachment from a given FoxKit BoneAttachment.
        /// </summary>
        /// <param name="boneAttachment">The FoxKit BoneAttachment.</param>
        /// <param name="fileHashManager">An StrCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxLib BoneAttachment.</returns>
        public static FoxLib.FormVariation.BoneAttachment MakeFoxLibBoneAttachment(BoneAttachment boneAttachment, StrCode64HashManager fileHashManager)
        {
            StrCode64StringPair modelFileName = boneAttachment.ModelFileName;
            StrCode64StringPair frdvFileName = boneAttachment.FrdvFileName;
            StrCode64StringPair simFileName = boneAttachment.SimFileName;

            ulong modelFileHash;
            ulong? frdvFileHash = null;
            ulong? simFileHash = null;


            modelFileHash = fileHashManager.GetHashFromStringPair(modelFileName);

            if (frdvFileName.IsUnhashed == IsStringOrHash.String)
            {
                if (frdvFileName.String != string.Empty)
                {
                    frdvFileHash = fileHashManager.GetHashFromStringPair(frdvFileName);
                }
            }
            else if (frdvFileName.IsUnhashed == IsStringOrHash.Hash)
            {
                if (frdvFileName.Hash != 0)
                {
                    frdvFileHash = frdvFileName.Hash;
                }
            }

            if (simFileName.IsUnhashed == IsStringOrHash.String)
            {
                if (simFileName.String != string.Empty)
                {
                    simFileHash = fileHashManager.GetHashFromStringPair(simFileName);
                }
            }
            else if (simFileName.IsUnhashed == IsStringOrHash.Hash)
            {
                if (simFileName.Hash != 0)
                {
                    simFileHash = simFileName.Hash;
                }
            }

            return new FoxLib.FormVariation.BoneAttachment(modelFileHash, frdvFileHash, simFileHash);
        }
    }
}