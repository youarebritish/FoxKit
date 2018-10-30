namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Configuration;

    using FoxKit.Core;

    using OdinSerializer;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// A reference to an Entity, which may or may not exist in a separate DataSet.
    /// </summary>
    [Serializable]
    public class EntityLink
    {
        public string PackagePath => this.packagePath;

        public string ArchivePath
        {
            get
            {
                return this.archivePath;
            }
            set
            {
                this.archivePath = value;
            }
        }
        
        public string NameInArchive => this.nameInArchive;

        public ulong Address => this.address;

        public Data Entity
        {
            get
            {
                return this.IsDataIdentifierEntityLink ? this.dataIdentifier?.Links[this.NameInArchive].Entity : this.referencedEntity;
            }

            set
            {
                this.referencedEntity = value;
                this.dataIdentifier = null;

                // TODO Set packagePath, archivePath, address
                this.packagePath = null;
                this.archivePath = null;
            }
        }

        public DataIdentifier DataIdentifier => this.dataIdentifier;

        public void SetDataIdentifier(DataIdentifier dataIdentifier, string key)
        {
            Assert.IsNotNull(dataIdentifier);
            Assert.IsFalse(string.IsNullOrEmpty(key));
            Assert.IsNotNull(dataIdentifier.Links);
            Assert.IsTrue(dataIdentifier.Links.ContainsKey(key));

            this.dataIdentifier = dataIdentifier;
            this.Entity = dataIdentifier.Links[key].Entity;

            this.packagePath = "DATA_IDENTIFIER";
            this.archivePath = dataIdentifier.Identifier;
            this.nameInArchive = key;
        }

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
        /// The referenced Entity.
        /// </summary>
        [OdinSerialize, NonSerialized]
        private Data referencedEntity;

        /// <summary>
        /// The referenced DataIdentifier, if any.
        /// </summary>
        [OdinSerialize, NonSerialized]
        private DataIdentifier dataIdentifier;

        public EntityLink()
        {
        }

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
        public EntityLink(string packagePath, string archivePath, string nameInArchive, ulong address)
        {
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
            // TODO 
            return false;
            /*
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
            DataSet referencedDataSet = null;
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
                this.referencedEntity = null; // TODO dataSet[this.address];
            }
            else
            {
                this.referencedEntity = dataSet.GetData(this.nameInArchive);
            }

            return this.referencedEntity != null;*/
        }
    }
}