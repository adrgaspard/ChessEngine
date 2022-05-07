using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Abstractions
{
    public abstract class EvaluatorBase : IEvaluator
    {
        public IPieceEvaluator PieceEvaluator { get; protected init; }

        protected EvaluatorBase(IPieceEvaluator pieceEvaluator)
        {
            PieceEvaluator = pieceEvaluator;
        }

        public abstract int Evaluate(Game game, Colour aiSide);

        protected int CountMaterial(Board board, Colour colour)
        {
            int material = 0;
            material += board[PieceType.Pawn, colour].CurrentSize * PieceEvaluator.GetValue(PieceType.Pawn);
            material += board[PieceType.Knight, colour].CurrentSize * PieceEvaluator.GetValue(PieceType.Knight);
            material += board[PieceType.Bishop, colour].CurrentSize * PieceEvaluator.GetValue(PieceType.Bishop);
            material += board[PieceType.Rook, colour].CurrentSize * PieceEvaluator.GetValue(PieceType.Rook);
            material += board[PieceType.Queen, colour].CurrentSize * PieceEvaluator.GetValue(PieceType.Queen);
            return material;
        }
    }
}
