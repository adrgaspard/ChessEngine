using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Contracts
{
    public interface ISorter
    {
        IList<Movement> Sort(Game game, AttackData attackData, IList<Movement> movements);
    }
}
