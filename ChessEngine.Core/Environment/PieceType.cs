namespace ChessEngine.Core.Environment
{
    [Flags]
    public enum PieceType : byte
    {
        None = 0,
        King = 1,
        Pawn = 2,
        Knight = 4,
        Bishop = 8,
        Rook = 16,
        Queen = 32
    }
}
