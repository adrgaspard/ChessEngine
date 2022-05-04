using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;

namespace ChessEngine.Core.Interactions.Contracts
{
    public interface IMovementGenerator
    {
        IList<Movement> GenerateMovements(Game game, AttackData attackData);
    }
}
