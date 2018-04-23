using System.IO;
using System.Text;

namespace FoxTool.Fox
{
    public abstract class FoxStringLiteralBase
    {
        public FoxStringLiteralBase()
        {
        }

        public FoxStringLiteralBase(string literal, FoxHash hash)
        {
            Literal = literal;
            Hash = hash;
        }

        public FoxHash Hash { get; set; }
        public string Literal { get; set; }
        public byte[] EncryptedLiteral { get; set; }

        public bool IsEncrypted
        {
            get { return EncryptedLiteral != null; }
        }

        public void Resolve(FoxLookupTable lookupTable)
        {
            Literal = lookupTable.Lookup(Hash.HashValue);
        }

        public void CalculateHash()
        {
            Hash = Literal == null ? Hash : new FoxHash {HashValue = Hashing.HashString(Literal)};
        }

        protected bool Equals(FoxStringLookupLiteral other)
        {
            return Equals(Hash, other.Hash) && string.Equals(Literal, other.Literal) &&
                   Equals(EncryptedLiteral, other.EncryptedLiteral);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FoxStringLookupLiteral) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Hash != null ? Hash.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Literal != null ? Literal.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (EncryptedLiteral != null ? EncryptedLiteral.GetHashCode() : 0);
                return hashCode;
            }
        }

        public void CheckForEncryption()
        {
            ulong literalHash = Hashing.HashString(Literal);
            if (literalHash != Hash.HashValue)
            {
                EncryptedLiteral = Constants.StringEncoding.GetBytes(Literal);
            }
        }

        public abstract void Write(Stream output);
    }
}
