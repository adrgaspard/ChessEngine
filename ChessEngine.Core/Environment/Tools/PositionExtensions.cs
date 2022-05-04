namespace ChessEngine.Core.Environment.Tools
{
    public static class PositionExtensions
    {
        public static bool IsOnBoard(this Position source)
        {
            return source.Rank >= 0 && source.Rank < BoardConsts.BoardSize && source.File >= 0 && source.File < BoardConsts.BoardSize;
        }

        public static Position ToDirection(this Position source)
        {
            return (Math.Abs(source.Rank) == Math.Abs(source.File) || source.File == 0 || source.Rank == 0)
                ? new Position(source.Rank >= 1 ? (sbyte)1 : (source.Rank <= -1 ? (sbyte)-1 : (sbyte)0), source.File >= 1 ? (sbyte)1 : (source.File <= -1 ? (sbyte)-1 : (sbyte)0))
                : source;
        }
    }
}
