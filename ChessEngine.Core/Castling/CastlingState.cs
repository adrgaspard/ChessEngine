namespace ChessEngine.Core.Castling
{
    [Flags]
    public enum CastlingState : byte
    {
        None = 0,
        WhiteCanCastleOnKingSide = 1,
        WhiteCanCastleOnQueenSide = 2,
        BlackCanCastleOnKingSide = 4,
        BlackCanCastleOnQueenSide = 8,
    }
}
