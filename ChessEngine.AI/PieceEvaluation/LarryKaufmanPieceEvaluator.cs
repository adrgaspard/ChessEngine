using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment;

namespace ChessEngine.AI.PieceEvaluation
{
    public class LarryKaufmanPieceEvaluator : IPieceEvaluator
    {
        public const int PawnValue = 100;
        public const int KnightValue = 350;
        public const int BishopValue = 350;
        public const int RookValue = 525;
        public const int QueenValue = 1000;

        public int GetValue(PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.Pawn => PawnValue,
                PieceType.Knight => KnightValue,
                PieceType.Bishop => BishopValue,
                PieceType.Rook => RookValue,
                PieceType.Queen => QueenValue,
                _ => 0,
            };
        }
    }
}
