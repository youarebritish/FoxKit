namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;

    using FoxKit.Core;

    using UnityEngine;

    /// <summary>
    /// A reference to an Entity, which may or may not exist in a separate DataSet.
    /// </summary>
    [Serializable]
    public class EntityLink
    {
        public string PackagePath => this.packagePath;

        public string ArchivePath => this.archivePath;
        
        public string NameInArchive => this.nameInArchive;

        public ulong Address => this.address;

            /// <summary>
        /// The package the referenced Entity belongs to. If empty, it belongs to the same package as the owning Entity.
        /// It may also have the value "DATA_IDENTIFIER", indicating that instead of looking for a file, it should look for a DataIdentifier.
        /// </summary>
        [SerializeField]
        private string packagePath;

        /// <summary>
        /// The archive (.fox2) the referenced Entity belongs to. If empty, it belongs to the same archive as the owning Entity.
        /// If this EntityLink is referencing a DataIdentifier, this will instead be the identifier field of the DataIdentifier.
        /// </summary>
        [SerializeField]
        private string archivePath;

        /// <summary>
        /// The name of the referenced Entity. If empty, it's referenced by address.
        /// If this EntityLink is referencing a DataIdentifier, this will be the key in the DataIdentifier to look for.
        /// </summary>
        [SerializeField]
        private string nameInArchive;

        /// <summary>
        /// The address of the referenced Entity.
        /// </summary>
        [SerializeField]
        private ulong address;

        /// <summary>
        /// The DataSet to which the owning Entity belongs.
        /// </summary>
        [SerializeField]
        private DataSet owningDataSet;

        /// <summary>
        /// The referenced Entity.
        /// </summary>
        [SerializeField]
        private Entity referencedEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityLink"/> class.
        /// </summary>
        /// <param name="owningDataSet">
        /// The DataSet to which the owning Entity belongs.
        /// </param>
        /// <param name="packagePath">
        /// The package the referenced Entity belongs to. If empty, it belongs to the same package as the owning Entity.
        /// </param>
        /// <param name="archivePath">
        /// The archive (.fox2) the referenced Entity belongs to. If empty, it belongs to the same archive as the owning Entity.
        /// </param>
        /// <param name="nameInArchive">
        /// The name of the referenced Entity. If empty, it's referenced by address.
        /// </param>
        /// <param name="address">
        /// The address of the referenced Entity.
        /// </param>
        public EntityLink(DataSet owningDataSet, string packagePath, string archivePath, string nameInArchive, ulong address)
        {
            this.owningDataSet = owningDataSet;
            this.packagePath = packagePath;
            this.archivePath = archivePath;
            this.nameInArchive = nameInArchive;
            this.address = address;
        }

        /// <summary>
        /// Is this EntityLink referencing a DataIdentifier?
        /// </summary>
        public bool IsDataIdentifierEntityLink => this.packagePath == "DATA_IDENTIFIER";

        /// <summary>
        /// Search for the referenced Entity.
        /// </summary>
        /// <param name="tryGetAsset">
        /// Function to load another asset.
        /// </param>
        /// <returns>
        /// True if the referenced Entity was found, else false.
        /// </returns>
        public bool ResolveReference(AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            // If all fields are empty, not really much we can do. It's not really looking for an Entity.
            if (string.IsNullOrEmpty(this.packagePath)
                && string.IsNullOrEmpty(this.archivePath)
                && string.IsNullOrEmpty(this.nameInArchive)
                && this.address == 0)
            {
                return true;
            }
            
            // For now, just ignore DataIdentifiers.
            if (this.IsDataIdentifierEntityLink)
            {
                this.referencedEntity = null;
                return false;
            }

            // If ArchivePath is empty, get the DataSet it belongs to.
            UnityEngine.Object referencedDataSet = null;
            if (string.IsNullOrEmpty(this.archivePath))
            {
                referencedDataSet = this.owningDataSet;
            }
            else
            {
                tryGetAsset(this.archivePath, out referencedDataSet);
            }

            var dataSet = referencedDataSet as DataSet;
            if (dataSet == null)
            {
                this.referencedEntity = null;
                return false;
            }

            if (string.IsNullOrEmpty(this.nameInArchive))
            {
                this.referencedEntity = dataSet[this.address];
            }
            else
            {
                this.referencedEntity = dataSet[this.nameInArchive];
            }

            return this.referencedEntity != null;
        }
    }
}