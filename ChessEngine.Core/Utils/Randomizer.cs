namespace ChessEngine.Core.Utils
{
    public static class Randomizer
    {
        public static readonly Random Instance = new();

        public static ulong NextUnsignedInt64(this Random source)
        {
            byte[] buffer = new byte[8];
            source.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}
