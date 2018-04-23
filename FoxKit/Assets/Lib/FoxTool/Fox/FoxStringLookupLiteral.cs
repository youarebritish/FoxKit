using System.IO;
using System.Text;

namespace FoxTool.Fox
{
    public class FoxStringLookupLiteral : FoxStringLiteralBase
    {
        public FoxStringLookupLiteral()
        {
        }

        public FoxStringLookupLiteral(FoxStringLiteralBase stringLiteral)
        {
            Literal = stringLiteral.Literal;
            Hash = stringLiteral.Hash;
            EncryptedLiteral = stringLiteral.EncryptedLiteral;
        }

        public FoxStringLookupLiteral(string literal, FoxHash hash) : base(literal, hash)
        {
        }

        public static FoxStringLookupLiteral ReadFoxStringLookupLiteral(Stream input)
        {
            FoxStringLookupLiteral stringLookupLiteral = new FoxStringLookupLiteral();
            stringLookupLiteral.Read(input);
            return stringLookupLiteral.Hash.HashValue == 0 ? null : stringLookupLiteral;
        }

        private void Read(Stream input)
        {
            BinaryReader reader = new BinaryReader(input, Encoding.Default, true);
            Hash = FoxHash.ReadFoxHash(input);

            if (Hash.HashValue == 0)
                return;
            int stringLength = reader.ReadInt32();
            Literal = reader.ReadString(stringLength);
        }

        public override void Write(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output, Encoding.Default, true);

            byte[] nameBytes = Literal == null ? new byte[0] : Constants.StringEncoding.GetBytes(Literal);

            Hash.Write(output);
            writer.Write((uint) nameBytes.Length);
            writer.Write(nameBytes);
        }
    }
}
