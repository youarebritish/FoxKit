namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using FoxKit.Core;

    /// <summary>
    /// A form variation operation used by the Fox Engine used to attach one model, and, optionally, its auxilliary files, to another via bones.
    /// </summary>
    [System.Serializable]
    public struct BoneAttachment
    {
        public PathFileNameCode64StringPair ModelFileName;
        public PathFileNameCode64StringPair FrdvFileName;
        public PathFileNameCode64StringPair SimFileName;

        /// <summary>
        /// Initializes a new instance of the BoneAttachment struct.
        /// </summary>
        /// <param name="modelFileName">Model file name.</param>
        /// <param name="frdvFileName">Frdv file name.</param>
        /// <param name="simFileName">Sim file name.</param>
        public BoneAttachment(PathFileNameCode64StringPair modelFileName, PathFileNameCode64StringPair frdvFileName, PathFileNameCode64StringPair simFileName)
        {
            this.ModelFileName = modelFileName;

            this.FrdvFileName = frdvFileName;

            this.SimFileName = simFileName;
        }

        /// <summary>
        /// Creates a FoxKit BoneAttachment from a given FoxLib BoneAttachment.
        /// </summary>
        /// <param name="boneAttachment">The FoxLib BoneAttachment.</param>
        /// <param name="fileHashManager">An PathFileNameCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxKit BoneAttachment.</returns>
        public static BoneAttachment MakeFoxKitBoneAttachment(FoxLib.FormVariation.BoneAttachment boneAttachment, PathFileNameCode64HashManager fileHashManager)
        {
            ulong modelFileHash = boneAttachment.ModelFileHash;
            ulong? frdvFileHash = boneAttachment.FrdvFileHash;
            ulong? simFileHash = boneAttachment.SimFileHash;

            PathFileNameCode64StringPair modelFileName;
            PathFileNameCode64StringPair frdvFileName;
            PathFileNameCode64StringPair simFileName;

            modelFileName = fileHashManager.GetStringPairFromUnhashAttempt(modelFileHash);

            if (frdvFileHash != null)
            {
                frdvFileName = fileHashManager.GetStringPairFromUnhashAttempt(frdvFileHash.Value);
            }
            else
            {
                frdvFileName = new PathFileNameCode64StringPair(string.Empty, IsStringOrHash.String);
            }

            if (simFileHash != null)
            {
                simFileName = fileHashManager.GetStringPairFromUnhashAttempt(simFileHash.Value);
            }
            else
            {
                simFileName = new PathFileNameCode64StringPair(string.Empty, IsStringOrHash.String);
            }

            return new BoneAttachment(modelFileName, frdvFileName, simFileName);
        }

        /// <summary>
        /// Creates a FoxLib BoneAttachment from a given FoxKit BoneAttachment.
        /// </summary>
        /// <param name="boneAttachment">The FoxKit BoneAttachment.</param>
        /// <param name="fileHashManager">An PathFileNameCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxLib BoneAttachment.</returns>
        public static FoxLib.FormVariation.BoneAttachment MakeFoxLibBoneAttachment(BoneAttachment boneAttachment, PathFileNameCode64HashManager fileHashManager)
        {
            PathFileNameCode64StringPair modelFileName = boneAttachment.ModelFileName;
            PathFileNameCode64StringPair frdvFileName = boneAttachment.FrdvFileName;
            PathFileNameCode64StringPair simFileName = boneAttachment.SimFileName;

            ulong modelFileHash;
            ulong? frdvFileHash = null;
            ulong? simFileHash = null;

            if (modelFileName.IsUnhashed == IsStringOrHash.String)
            {
                if (modelFileName.String != string.Empty)
                {
                    modelFileHash = fileHashManager.GetHashFromStringPair(modelFileName);
                }
                else
                {
                    throw new System.Exception("Error: The Model File Name field must have a valid name!");
                }
            }
            else
            {
                if (modelFileName.Hash != 0)
                {
                    modelFileHash = modelFileName.Hash;
                }
                else
                {
                    throw new System.Exception("Error: The Model File Name field must have a valid name!");
                }
            }

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