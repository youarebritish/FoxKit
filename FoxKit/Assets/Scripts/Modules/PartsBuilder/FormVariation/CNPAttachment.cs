namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using FoxKit.Core;
          
    /// <summary>
    /// A form variation operation used by the Fox Engine used to attach one model, and, optionally, its auxilliary files, to another via a connection point (CNP).
    /// </summary>
    [System.Serializable]
    public struct CNPAttachment
    {
        public StrCode32StringPair CNPName;
        public PathFileNameCode64StringPair ModelFileName;
        public PathFileNameCode64StringPair FrdvFileName;
        public PathFileNameCode64StringPair SimFileName;

        /// <summary>
        /// Initializes a new instance of the CNPAttachment struct.
        /// </summary>
        /// <param name="CNPName">CNP (connection point) name.</param>
        /// <param name="modelFileName">Model file name.</param>
        /// <param name="frdvFileName">Frdv file name.</param>
        /// <param name="simFileName">Sim file name.</param>
        public CNPAttachment(StrCode32StringPair CNPName, PathFileNameCode64StringPair modelFileName, PathFileNameCode64StringPair frdvFileName, PathFileNameCode64StringPair simFileName)
        {
            this.CNPName = CNPName;

            this.ModelFileName = modelFileName;

            this.FrdvFileName = frdvFileName;

            this.SimFileName = simFileName;
        }

        /// <summary>
        /// Creates a FoxKit CNPAttachment from a given FoxLib CNPAttachment.
        /// </summary>
        /// <param name="CNPAttachment">The FoxLib CNPAttachment.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <param name="fileHashManager">An PathFileNameCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxKit CNPAttachment.</returns>
        public static CNPAttachment MakeFoxKitCNPAttachment(FoxLib.FormVariation.CNPAttachment CNPAttachment, StrCode32HashManager nameHashManager, PathFileNameCode64HashManager fileHashManager)
        {
            uint CNPHash = CNPAttachment.CNPHash;
            ulong modelFileHash = CNPAttachment.ModelFileHash;
            ulong? frdvFileHash = CNPAttachment.FrdvFileHash;
            ulong? simFileHash = CNPAttachment.SimFileHash;

            StrCode32StringPair CNPName;
            PathFileNameCode64StringPair modelFileName;
            PathFileNameCode64StringPair frdvFileName;
            PathFileNameCode64StringPair simFileName;

            CNPName = nameHashManager.GetStringPairFromUnhashAttempt(CNPHash);

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

            return new CNPAttachment(CNPName, modelFileName, frdvFileName, simFileName);
        }

        /// <summary>
        /// Creates a FoxLib CNPAttachment from a given FoxKit CNPAttachment.
        /// </summary>
        /// <param name="CNPAttachment">The FoxKit CNPAttachment.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <param name="fileHashManager">An PathFileNameCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxLib CNPAttachment.</returns>
        public static FoxLib.FormVariation.CNPAttachment MakeFoxLibCNPAttachment(CNPAttachment CNPAttachment, StrCode32HashManager nameHashManager, PathFileNameCode64HashManager fileHashManager)
        {
            StrCode32StringPair CNPName = CNPAttachment.CNPName;
            PathFileNameCode64StringPair modelFileName = CNPAttachment.ModelFileName;
            PathFileNameCode64StringPair frdvFileName = CNPAttachment.FrdvFileName;
            PathFileNameCode64StringPair simFileName = CNPAttachment.SimFileName;

            uint CNPHash;
            ulong modelFileHash;
            ulong? frdvFileHash = null;
            ulong? simFileHash = null;

            if (CNPName.IsUnhashed == IsStringOrHash.String && modelFileName.IsUnhashed == IsStringOrHash.String)
            {
                if (CNPName.String != string.Empty && modelFileName.String != string.Empty)
                {
                    CNPHash = nameHashManager.GetHashFromStringPair(CNPName);

                    modelFileHash = fileHashManager.GetHashFromStringPair(modelFileName);
                }
                else
                {
                    throw new System.Exception("Error: Both the CNP Name and Model File Name fields must have valid names!");
                }
            }
            else
            {
                if (CNPName.Hash != 0 && modelFileName.Hash != 0)
                {
                    CNPHash = CNPName.Hash;

                    modelFileHash = modelFileName.Hash;
                }
                else
                {
                    throw new System.Exception("Error: Both the CNP Name and Model File Name fields must have valid names!");
                }
            }

            if (frdvFileName.IsUnhashed == IsStringOrHash.String)
            {
                if (frdvFileName.String != string.Empty)
                {
                    frdvFileHash = fileHashManager.GetHashFromStringPair(frdvFileName);
                }
            }
            else
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
            else
            {
                if (simFileName.Hash != 0)
                {
                    simFileHash = simFileName.Hash;
                }
            }

            return new FoxLib.FormVariation.CNPAttachment(CNPHash, modelFileHash, frdvFileHash, simFileHash);
        }
    }
}