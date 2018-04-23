using System;
using System.IO;

namespace FoxTool.Fox
{
    public class FoxStringLiteral : FoxStringLiteralBase
    {
        public FoxStringLiteral()
        {
        }

        public FoxStringLiteral(string literal, FoxHash hash)
            : base(literal, hash)
        {
        }

        public override void Write(Stream output)
        {
            Hash.Write(output);
        }

        public static FoxStringLiteral ReadStringLiteral(Stream input)
        {
            FoxStringLiteral foxStringLiteral = new FoxStringLiteral();
            foxStringLiteral.Read(input);
            return foxStringLiteral;
        }

        private void Read(Stream input)
        {
            Hash = FoxHash.ReadFoxHash(input);
        }

        public static int Size()
        {
            return FoxHash.Size;
        }

        public override string ToString()
        {
            return Literal ?? String.Format("0x{0:X8}", Hash.HashValue);
        }
    }
}
