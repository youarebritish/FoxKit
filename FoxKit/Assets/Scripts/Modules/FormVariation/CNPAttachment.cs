namespace FoxKit.Modules.FormVariation
{
    using FoxKit.Core;
    using FoxKit.Utils;
          
    /// <summary>
    /// A form variation operation used by the Fox Engine used to attach one model, and, optionally, its auxilliary files, to another via a connection point (CNP).
    /// </summary>
    [System.Serializable]
    public struct CNPAttachment
    {
        public StringHashPair CNPName;
        public StringHashPair ModelFileName;
        public StringHashPair FrdvFileName;
        public StringHashPair SimFileName;

        /// <summary>
        /// Creates a FoxKit CNPAttachment from a FoxLib CNPAttachment.
        /// </summary>
        /// <param name="CNPAttachment">The FoxLib CNPAttachment to convert.</param>
        /// <returns>The created FoxKit CNPAttachment.</returns>
        public CNPAttachment(FoxLib.FormVariation.CNPAttachment CNPAttachment)
        {
            uint CNPHash = CNPAttachment.CNPHash;
            ulong modelFileHash = CNPAttachment.ModelFileHash;
            ulong? frdvFileHash = CNPAttachment.FrdvFileHash;
            ulong? simFileHash = CNPAttachment.SimFileHash;

            string CNPName;

            if (Hashing.TryGetFileNameFromHash(CNPHash, out CNPName) == true)
            {
                this.CNPName.Name = CNPName;
                this.CNPName.IsHash = false;
            }
            else
            {
                this.CNPName.Name = CNPHash.ToString();
                this.CNPName.IsHash = true;
            }

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
                    this.SimFileName.Name = simFileHash.Value.ToString("X");
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