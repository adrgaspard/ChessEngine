namespace ChessEngine.Core.Interactions
{
    [Flags]
    public enum MovementFlag : byte
    {
        None = 0,

        PawnPush = 1,
        PawnEnPassantCapture = 2,
        PawnPromotionToBishop = 4,
        PawnPromotionToKnight = 8,
        PawnPromotionToQueen = 16,
        PawnPromotionToRook = 32,
        PawnAllPromotions = 60,

        KingCastling = 64
    }
}
