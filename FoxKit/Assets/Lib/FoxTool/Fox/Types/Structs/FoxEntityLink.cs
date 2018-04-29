using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace FoxTool.Fox.Types.Structs
{
    public class FoxEntityLink : FoxStruct
    {
        public ulong EntityHandle { get; set; }
        public FoxStringLiteral PackagePathLiteral { get; set; }
        public FoxStringLiteral ArchivePathLiteral { get; set; }
        public FoxStringLiteral NameInArchiveLiteral { get; set; }

        public override void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            PackagePathLiteral = FoxStringLiteral.ReadStringLiteral(input);
            ArchivePathLiteral = FoxStringLiteral.ReadStringLiteral(input);
            NameInArchiveLiteral = FoxStringLiteral.ReadStringLiteral(input);
            EntityHandle = reader.ReadUInt64();
        }

        public override void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);
            PackagePathLiteral.Write(output);
            ArchivePathLiteral.Write(output);
            NameInArchiveLiteral.Write(output);
            writer.Write(EntityHandle);
        }

        public override int Size()
        {
            return 3*FoxStringLiteral.Size() + sizeof (ulong);
        }

        public override void ResolveStringLiterals(FoxLookupTable lookupTable)
        {
            PackagePathLiteral.Resolve(lookupTable);
            ArchivePathLiteral.Resolve(lookupTable);
            NameInArchiveLiteral.Resolve(lookupTable);
        }

        public override void CalculateHashes()
        {
            PackagePathLiteral.CalculateHash();
            ArchivePathLiteral.CalculateHash();
            NameInArchiveLiteral.CalculateHash();
        }

        public override void CollectStringLookupLiterals(List<FoxStringLookupLiteral> literals)
        {
            literals.Add(new FoxStringLookupLiteral(PackagePathLiteral));
            literals.Add(new FoxStringLookupLiteral(NameInArchiveLiteral));
            literals.Add(new FoxStringLookupLiteral(NameInArchiveLiteral));
        }

        public override void ReadXml(XmlReader reader)
        {
            var isEmptyElement = reader.IsEmptyElement;

            var packagePathHash = new FoxHash("packagePathHash");
            packagePathHash.ReadXml(reader);
            var archivePathHash = new FoxHash("archivePathHash");
            archivePathHash.ReadXml(reader);
            var nameInArchiveHash = new FoxHash("nameInArchiveHash");
            nameInArchiveHash.ReadXml(reader);

            var packagePath = reader.GetAttribute("packagePath");
            var archivePath = reader.GetAttribute("archivePath");
            var nameInArchive = reader.GetAttribute("nameInArchive");

            PackagePathLiteral = new FoxStringLiteral(packagePath, packagePathHash);
            ArchivePathLiteral = new FoxStringLiteral(archivePath, archivePathHash);
            NameInArchiveLiteral = new FoxStringLiteral(nameInArchive, nameInArchiveHash);

            reader.ReadStartElement("value");
            if (isEmptyElement == false)
            {
                string value = reader.ReadString();
                EntityHandle = value.StartsWith("0x")
                    ? ulong.Parse(value.Substring(2, value.Length - 2), NumberStyles.AllowHexSpecifier)
                    : ulong.Parse(value);
                reader.ReadEndElement();
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            // HACK: HashName should should only be set once. Either in ReadFoxHash or the constructor of FoxHash.
            PackagePathLiteral.Hash.HashName = "packagePathHash";
            ArchivePathLiteral.Hash.HashName = "archivePathHash";
            NameInArchiveLiteral.Hash.HashName = "nameInArchiveHash";

            if (PackagePathLiteral.Literal == null)
                PackagePathLiteral.Hash.WriteXml(writer);
            else
                writer.WriteAttributeString("packagePath", PackagePathLiteral.Literal);

            if (ArchivePathLiteral.Literal == null)
                ArchivePathLiteral.Hash.WriteXml(writer);
            else
                writer.WriteAttributeString("archivePath", ArchivePathLiteral.Literal);

            if (NameInArchiveLiteral.Literal == null)
                NameInArchiveLiteral.Hash.WriteXml(writer);
            else
                writer.WriteAttributeString("nameInArchive", NameInArchiveLiteral.Literal);

            writer.WriteString(String.Format("0x{0:X8}", EntityHandle));
        }

        public override string ToString()
        {
            string packagePath = PackagePathLiteral.Literal ??
                                 String.Format("0x{0:X8}", PackagePathLiteral.Hash.HashValue);
            string archivePath = ArchivePathLiteral.Literal ??
                                 String.Format("0x{0:X8}", ArchivePathLiteral.Hash.HashValue);
            string nameInArchive = NameInArchiveLiteral.Literal ??
                                   String.Format("0x{0:X8}", NameInArchiveLiteral.Hash.HashValue);
            return
                string.Format(
                    "packagePath=\"{0}\", archivePath=\"{1}\", nameInArchive=\"{2}\", EntityHandle=\"0x{3:X8}\"",
                    packagePath, archivePath, nameInArchive, EntityHandle);
        }
    }
}
