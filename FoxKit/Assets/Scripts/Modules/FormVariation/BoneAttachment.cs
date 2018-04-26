namespace FoxKit.Modules.FormVariation
{
    using FoxKit.Core;
    using FoxKit.Utils;
            
    /// <summary>
    /// A form variation operation used by the Fox Engine used to attach one model, and, optionally, its auxilliary files, to another via bones.
    /// </summary>
    [System.Serializable]
    public struct BoneAttachment
    {
        public StringHashPair ModelFileName;
        public StringHashPair FrdvFileName;
        public StringHashPair SimFileName;

        /// <summary>
        /// Creates a FoxKit BoneAttachment from a FoxLib BoneAttachment.
        /// </summary>
        /// <param name="boneAttachment">The FoxLib BoneAttachment to convert.</param>
        /// <returns>The created FoxKit BoneAttachment.</returns>
        public BoneAttachment(FoxLib.FormVariation.BoneAttachment boneAttachment)
        {
            ulong modelFileHash = boneAttachment.ModelFileHash;
            ulong? frdvFileHash = boneAttachment.FrdvFileHash;
            ulong? simFileHash = boneAttachment.SimFileHash;

            string modelFileName;

            if (Hashing.TryGetFileNameFromHash(modelFileHash, out modelFileName) == true)
            {
                this.ModelFileName.Name = modelFileName;
                this.ModelFileName.IsHash = false;
            }
            else
            {
                this.ModelFileName.Name = modelFileHash.ToString();
                this.ModelFileName.IsHash = true;
            }

            string frdvFileName;

            if (frdvFileHash != null)
            {
                if (Hashing.TryGetFileNameFromHash(frdvFileHash.Value, out frdvFileName) == true)
                {
                    this.FrdvFileName.Name = frdvFileName;
                    this.FrdvFileName.IsHash = false;
                }
                else
                {
                    this.FrdvFileName.Name = frdvFileHash.Value.ToString();
                    this.FrdvFileName.IsHash = true;
                }
            }
            else
            {
                this.FrdvFileName.Name = string.Empty;
                this.FrdvFileName.IsHash = false;
            }

            string simFileName;

            if (simFileHash != null)
            {
                if (Hashing.TryGetFileNameFromHash(simFileHash.Value, out simFileName) == true)
                {
                    this.SimFileName.Name = simFileName;
                    this.SimFileName.IsHash = false;
                }
                else
                {
                    this.SimFileName.Name = simFileHash.Value.ToString();
                    this.SimFileName.IsHash = true;
                }
            }
            else
            {
                this.SimFileName.Name = string.Empty;
                this.SimFileName.IsHash = false;
            }
        }
    }
}