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
        public StrCode64StringPair ModelFileName;
        public StrCode64StringPair FrdvFileName;
        public StrCode64StringPair SimFileName;

        /// <summary>
        /// Initializes a new instance of the CNPAttachment struct.
        /// </summary>
        /// <param name="CNPName">CNP (connection point) name.</param>
        /// <param name="modelFileName">Model file name.</param>
        /// <param name="frdvFileName">Frdv file name.</param>
        /// <param name="simFileName">Sim file name.</param>
        public CNPAttachment(StrCode32StringPair CNPName, StrCode64StringPair modelFileName, StrCode64StringPair frdvFileName, StrCode64StringPair simFileName)
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
        /// <param name="fileHashManager">An StrCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxKit CNPAttachment.</returns>
        public static CNPAttachment MakeFoxKitCNPAttachment(FoxLib.FormVariation.CNPAttachment CNPAttachment, StrCode32HashManager nameHashManager, StrCode64HashManager fileHashManager)
        {
            uint CNPHash = CNPAttachment.CNPHash;
            ulong modelFileHash = CNPAttachment.ModelFileHash;
            ulong? frdvFileHash = CNPAttachment.FrdvFileHash;
            ulong? simFileHash = CNPAttachment.SimFileHash;

            StrCode32StringPair CNPName;
            StrCode64StringPair modelFileName;
            StrCode64StringPair frdvFileName;
            StrCode64StringPair simFileName;

            CNPName = nameHashManager.GetStringPairFromUnhashAttempt(CNPHash);

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

            return new CNPAttachment(CNPName, modelFileName, frdvFileName, simFileName);
        }

        /// <summary>
        /// Creates a FoxLib CNPAttachment from a given FoxKit CNPAttachment.
        /// </summary>
        /// <param name="CNPAttachment">The FoxKit CNPAttachment.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <param name="fileHashManager">An StrCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxLib CNPAttachment.</returns>
        public static FoxLib.FormVariation.CNPAttachment MakeFoxLibCNPAttachment(CNPAttachment CNPAttachment, StrCode32HashManager nameHashManager, StrCode64HashManager fileHashManager)
        {
            StrCode32StringPair CNPName = CNPAttachment.CNPName;
            StrCode64StringPair modelFileName = CNPAttachment.ModelFileName;
            StrCode64StringPair frdvFileName = CNPAttachment.FrdvFileName;
            StrCode64StringPair simFileName = CNPAttachment.SimFileName;

            uint CNPHash;
            ulong modelFileHash;
            ulong? frdvFileHash = null;
            ulong? simFileHash = null;

            CNPHash = nameHashManager.GetHashFromStringPair(CNPName);

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

            return new FoxLib.FormVariation.CNPAttachment(CNPHash, modelFileHash, frdvFileHash, simFileHash);
        }
    }
}