namespace CityHash
{
    public class UInt256
    {
        public UInt256()
        {
        }

        public UInt256(UInt128 low, UInt128 high)
        {
            Low = low;
            High = high;
        }

        public UInt128 Low { get; set; }
        public UInt128 High { get; set; }

        protected bool Equals(UInt256 other)
        {
            return Equals(Low, other.Low) && Equals(High, other.High);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UInt128) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Low.GetHashCode()*397) ^ High.GetHashCode();
            }
        }
    }
}
