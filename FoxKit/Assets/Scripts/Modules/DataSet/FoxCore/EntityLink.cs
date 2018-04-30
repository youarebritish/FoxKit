using FoxKit.Core;
using System;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public class EntityLink
    {
        public string PackagePath;
        public string ArchivePath;
        public string NameInArchive;
        public ulong Address;

        public DataSet OwningDataSet;
        public Entity ReferencedEntity;

        public bool IsDataIdentifierEntityLink
        {
            get
            {
                return PackagePath == "DATA_IDENTIFIER";
            }
        }
        
        public EntityLink(DataSet owningDataSet, string packagePath, string archivePath, string nameInArchive, ulong address)
        {
            this.OwningDataSet = owningDataSet;
            this.PackagePath = packagePath;
            this.ArchivePath = archivePath;
            this.NameInArchive = nameInArchive;
            this.Address = address;
        }

        public bool ResolveReference(AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
            if (string.IsNullOrEmpty(PackagePath) && string.IsNullOrEmpty(ArchivePath) && string.IsNullOrEmpty(NameInArchive))
            {

            }

            // TODO: Figure out how to deal with this.
            if (IsDataIdentifierEntityLink)
            {
                ReferencedEntity = null;
                return false;
            }

            // If ArchivePath is empty, get the DataSet it belongs to.
            UnityEngine.Object referencedDataSet = null;
            if (string.IsNullOrEmpty(ArchivePath))
            {
                referencedDataSet = OwningDataSet;
            }
            else
            {
                tryGetImportedAsset(this.ArchivePath, out referencedDataSet);
            }

            var dataSet = referencedDataSet as DataSet;
            if (dataSet == null)
            {
                ReferencedEntity = null;
                return false;
            }

            if (string.IsNullOrEmpty(NameInArchive))
            {
                ReferencedEntity = dataSet.AddressMap[Address];
            }
            else
            {
                ReferencedEntity = dataSet.DataList[NameInArchive];
            }

            return ReferencedEntity != null;
        }
    }
}