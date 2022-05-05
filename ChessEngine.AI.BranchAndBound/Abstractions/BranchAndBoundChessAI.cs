using ChessEngine.Core.AI;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.BranchAndBound.Abstractions
{
    public abstract class BranchAndBoundChessAI : IChessAI
    {
        public abstract Task<Movement> SelectMovement(Game game, IReadOnlyList<Movement> legalMovements, CancellationToken cancellationToken);
    }
}
