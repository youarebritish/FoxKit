namespace FoxKit.Core
{
    /// <summary>
    /// Pairs a hash with its string representation to allow working with assets where the unhashed string is not always known.
    /// </summary>
    public interface IStringHashPair<THash> where THash : struct
    {
        string String { get; }
        THash Hash { get; }
        bool IsUnhashed { get; }
    }
}