using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;

namespace ChessEngine.Core.AI
{
    public interface IChessAI
    {
        Movement SelectMovement(Game game, IReadOnlyList<Movement> legalMovements, CancellationToken token);
    }
}
