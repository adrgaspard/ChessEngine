using ChessEngine.Core.Environment;

namespace ChessEngine.AI.Contracts
{
    public interface IPieceEvaluator
    {
        int GetValue(PieceType pieceType);
    }
}
